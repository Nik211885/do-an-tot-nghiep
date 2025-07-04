import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Password strength validator
 * @param minLength Minimum password length
 * @returns ValidatorFn for form control
 */
export function passwordStrengthValidator(minLength: number = 8): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) {
      return null;
    }

    const hasUpperCase = /[A-Z]/.test(value);
    const hasLowerCase = /[a-z]/.test(value);
    const hasNumeric = /[0-9]/.test(value);
    const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(value);
    const meetsMinLength = value.length >= minLength;

    const passwordValid = hasUpperCase && hasLowerCase && hasNumeric && hasSpecialChar && meetsMinLength;

    if (!passwordValid) {
      return {
        passwordStrength: {
          hasUpperCase,
          hasLowerCase,
          hasNumeric,
          hasSpecialChar,
          meetsMinLength
        }
      };
    }

    return null;
  };
}

/**
 * Password match validator for confirm password fields
 * @param passwordControlName The name of the password control to match
 * @returns ValidatorFn for form group
 */
export function passwordMatchValidator(passwordControlName: string): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const password = control.parent?.get(passwordControlName)?.value;
    const confirmPassword = control.value;

    return password === confirmPassword ? null : { passwordMismatch: true };
  };
}

/**
 * Email validator
 * @returns ValidatorFn for form control
 */
export function emailValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) {
      return null;
    }

    const validEmailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return validEmailPattern.test(control.value) ? null : { invalidEmail: true };
  };
}
