import type { IFormData } from './schema'; 

export interface IFormState {
  currentStep: number;
  formData: IFormData;
  errors: Partial<Record<keyof IFormData, string>>;
  isSubmitting: boolean;
}

export type TFormAction =
  | { type: 'UPDATE_FIELD'; payload: { field: keyof IFormData; value: string | boolean } }
  | { type: 'SET_ERROR'; payload: Partial<Record<keyof IFormData, string>> }
  | { type: 'NEXT_STEP' }
  | { type: 'PREV_STEP' }
  | { type: 'SUBMIT_START' }
  | { type: 'SUBMIT_SUCCESS' };