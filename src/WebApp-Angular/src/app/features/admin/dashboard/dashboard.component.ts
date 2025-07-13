import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, Subscription } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { AuthService } from '../../../core/auth/auth.service';
import { UserModel } from '../../../core/models/user.model';

interface WeatherData {
  location: string;
  temperature: number;
  description: string;
  humidity: number;
  windSpeed: number;
  icon: string;
  country: string;
}

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
  standalone: true
})
export class DashboardComponent implements OnInit, OnDestroy {
  userModel!: UserModel;
  weatherData: WeatherData | null = null;
  isWeatherLoading = false;
  weatherError: string | null = null;
  currentTime = new Date();

  private apiKey = '7699b723f8870a3e2523b4e469d861fc';

  // Sử dụng proxy hoặc CORS-enabled endpoint
  private weatherBaseUrl = '/api/weather/weather'; // Sử dụng proxy
  // Hoặc sử dụng alternative endpoint:
  // private weatherBaseUrl = 'https://api.openweathermap.org/data/2.5/weather';

  private timeInterval?: any;
  private subscriptions: Subscription[] = [];

  constructor(
    public authService: AuthService,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.loadUserProfile();
    this.loadWeather();
    this.updateTime();
    this.startTimeUpdate();
  }

  ngOnDestroy(): void {
    // Cleanup subscriptions và intervals
    this.subscriptions.forEach(sub => sub.unsubscribe());
    if (this.timeInterval) {
      clearInterval(this.timeInterval);
    }
  }

  private loadUserProfile(): void {
    const authSub = this.authService.initialize().subscribe({
      next: data => {
        if (data) {
          const profileSub = this.authService.loadUserProfile().subscribe({
            next: userData => {
              this.userModel = userData;
            },
            error: error => {
              console.error('Error loading user profile:', error);
            }
          });
          this.subscriptions.push(profileSub);
        }
      },
      error: error => {
        console.error('Error initializing auth:', error);
      }
    });
    this.subscriptions.push(authSub);
  }

  private loadWeather(): void {
    if (!this.apiKey) {
      this.weatherError = 'API key không được cấu hình';
      return;
    }

    this.isWeatherLoading = true;
    this.weatherError = null;

    const weatherSub = this.getCurrentLocationWeather().subscribe({
      next: (data) => {
        this.weatherData = data;
        this.isWeatherLoading = false;
        this.weatherError = null;
      },
      error: (error) => {
        console.error('Error loading weather:', error);
        this.isWeatherLoading = false;
        this.weatherError = this.getWeatherErrorMessage(error);

        // Fallback to mock data if API fails
        this.loadMockWeatherData();
      }
    });
    this.subscriptions.push(weatherSub);
  }

  private getCurrentLocationWeather(): Observable<WeatherData> {
    return new Observable(observer => {
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
          position => {
            const lat = position.coords.latitude;
            const lon = position.coords.longitude;

            // Sử dụng proxy hoặc JSONP
            const url = `${this.weatherBaseUrl}?lat=${lat}&lon=${lon}&appid=${this.apiKey}&units=metric&lang=vi`;

            const locationSub = this.getWeatherWithCORSHandling(url).subscribe({
              next: data => {
                observer.next(data);
                observer.complete();
              },
              error: error => {
                // Fallback to Hanoi if location-based weather fails
                this.getWeatherByCity('Hanoi').subscribe({
                  next: data => {
                    observer.next(data);
                    observer.complete();
                  },
                  error: fallbackError => {
                    observer.error(fallbackError);
                  }
                });
              }
            });
          },
          error => {
            console.warn('Geolocation error, using default city:', error);
            // Nếu không lấy được vị trí, mặc định là Hà Nội
            const defaultSub = this.getWeatherByCity('Hanoi').subscribe({
              next: data => {
                observer.next(data);
                observer.complete();
              },
              error: error => {
                observer.error(error);
              }
            });
          },
          {
            timeout: 10000, // 10 seconds timeout
            enableHighAccuracy: false
          }
        );
      } else {
        console.warn('Geolocation not supported, using default city');
        const defaultSub = this.getWeatherByCity('Hanoi').subscribe({
          next: data => {
            observer.next(data);
            observer.complete();
          },
          error: error => {
            observer.error(error);
          }
        });
      }
    });
  }

  private getWeatherByCity(city: string): Observable<WeatherData> {
    const url = `${this.weatherBaseUrl}?q=${city}&appid=${this.apiKey}&units=metric&lang=vi`;
    return this.getWeatherWithCORSHandling(url);
  }

  private getWeatherWithCORSHandling(url: string): Observable<WeatherData> {
    // Thêm headers để xử lý CORS
    const headers = {
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*'
    };

    return this.http.get<any>(url, { headers }).pipe(
      map(response => this.mapWeatherResponse(response)),
      catchError(error => {
        console.error('CORS Error fetching weather:', error);

        // Nếu CORS error, thử sử dụng alternative method
        if (error.status === 0 || error.message.includes('CORS')) {
          return this.getWeatherAlternative(url);
        }

        throw error;
      })
    );
  }

  private getWeatherAlternative(originalUrl: string): Observable<WeatherData> {
    // Sử dụng JSONP hoặc alternative endpoint
    const corsProxyUrl = 'https://cors-anywhere.herokuapp.com/';
    const proxiedUrl = corsProxyUrl + originalUrl.replace('/api/weather/', 'https://api.openweathermap.org/data/2.5/');

    return this.http.get<any>(proxiedUrl).pipe(
      map(response => this.mapWeatherResponse(response)),
      catchError(error => {
        console.error('Alternative method also failed:', error);
        // Return observable with mock data
        return of(this.getMockWeatherData());
      })
    );
  }

  private getMockWeatherData(): WeatherData {
    return {
      location: 'Hanoi',
      temperature: 28,
      description: 'Có mây',
      humidity: 65,
      windSpeed: 3.2,
      icon: '02d',
      country: 'VN'
    };
  }

  private loadMockWeatherData(): void {
    // Fallback to mock data if API completely fails
    setTimeout(() => {
      this.weatherData = this.getMockWeatherData();
      this.isWeatherLoading = false;
      this.weatherError = null;
    }, 1000);
  }

  private mapWeatherResponse(response: any): WeatherData {
    return {
      location: response.name,
      temperature: Math.round(response.main.temp),
      description: response.weather[0].description,
      humidity: response.main.humidity,
      windSpeed: Math.round(response.wind.speed * 10) / 10, // Round to 1 decimal place
      icon: response.weather[0].icon,
      country: response.sys.country
    };
  }

  private updateTime(): void {
    this.currentTime = new Date();
  }

  private startTimeUpdate(): void {
    this.timeInterval = setInterval(() => {
      this.updateTime();
    }, 1000);
  }

  private getWeatherErrorMessage(error: any): string {
    if (error instanceof HttpErrorResponse) {
      switch (error.status) {
        case 0:
          return 'Lỗi CORS - Không thể kết nối đến server thời tiết';
        case 401:
          return 'API key không hợp lệ';
        case 404:
          return 'Không tìm thấy thông tin thời tiết';
        case 429:
          return 'Quá nhiều yêu cầu, vui lòng thử lại sau';
        default:
          return `Lỗi: ${error.status} - ${error.message}`;
      }
    }

    if (error.message && error.message.includes('CORS')) {
      return 'Lỗi CORS - Sử dụng dữ liệu mẫu';
    }

    return 'Có lỗi xảy ra khi tải thông tin thời tiết';
  }

  refreshWeather(): void {
    this.loadWeather();
  }

  getUserInitials(): string {
    if (!this.userModel) return '';
    const firstName = this.userModel.firstName || '';
    const lastName = this.userModel.lastName || '';
    return (firstName.charAt(0) + lastName.charAt(0)).toUpperCase();
  }

  hasUserAttributes(): boolean {
    return (this.userModel?.attributes && Object.keys(this.userModel.attributes).length > 0) ?? false;
  }

  getUserAttributesArray(): {key: string, value: any}[] {
    if (!this.userModel?.attributes) return [];
    return Object.entries(this.userModel.attributes).map(([key, value]) => ({
      key,
      value
    }));
  }

  getGreeting(): string {
    const hour = this.currentTime.getHours();
    if (hour < 12) return 'Chào buổi sáng';
    if (hour < 18) return 'Chào buổi chiều';
    return 'Chào buổi tối';
  }

  getWeatherIconUrl(): string {
    if (!this.weatherData?.icon) return '';
    return `https://openweathermap.org/img/wn/${this.weatherData.icon}@2x.png`;
  }

  getFormattedTime(): string {
    return this.currentTime.toLocaleTimeString('vi-VN', {
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit'
    });
  }

  getFormattedDate(): string {
    return this.currentTime.toLocaleDateString('vi-VN', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }
}
