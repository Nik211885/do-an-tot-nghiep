export function convertUtcToLocal(utcDateInput: Date | string) : Date{
    const utcDate = new Date(utcDateInput);
    return new Date(
        utcDate.getUTCFullYear(),
        utcDate.getUTCMonth(),
        utcDate.getUTCDate(),
        utcDate.getUTCHours(),
        utcDate.getUTCMinutes(),
        utcDate.getUTCSeconds()
      );
}

export function formatRelativeTime(dateInput: Date | string) : string{
    const inputDate = new Date(dateInput);
    const now = new Date();
    const diffMs = now.getTime() - inputDate.getTime();
    const diffMinutes = Math.floor(diffMs / (1000 * 60));
    const diffHours = Math.floor(diffMinutes / 60);
    const diffDays = Math.floor(diffHours / 24);
    if (diffMinutes < 1) {
        return "vừa xong";
      } else if (diffMinutes < 60) {
        return `${diffMinutes} phút trước`;
      } else if (diffHours < 24) {
        return `${diffHours} giờ trước`;
      } else if (diffDays <= 7) {
        return `${diffDays} ngày trước`;
      } else {
        // Format dd/MM/yyyy
        const dd = inputDate.getDate().toString().padStart(2, "0");
        const mm = (inputDate.getMonth() + 1).toString().padStart(2, "0");
        const yyyy = inputDate.getFullYear();
        return `${dd}/${mm}/${yyyy}`;
      }
    return "";
}

export function formatVietnameseDate(date: Date): string {
  const hours = date.getHours().toString().padStart(2, '0');
  const minutes = date.getMinutes().toString().padStart(2, '0');
  const day = date.getDate();
  const month = date.getMonth() + 1; 

  return `${hours}:${minutes}, ${day} tháng ${month}`;
}