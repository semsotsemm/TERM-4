// src/components/RegistrationForm/reducer.ts
import type { IFormState, TFormAction } from './types';

export const initialState: IFormState = {
  currentStep: 1,
  formData: {
    email: '',
    password: '',
    username: '',
    city: '',
    occupation: 'student',
    terms: undefined,
  },
  errors: {},
  isSubmitting: false,
};

export const registrationReducer = (state: IFormState, action: TFormAction): IFormState => {
  switch (action.type) {
    case 'UPDATE_FIELD':
      return {
        ...state,
        formData: { ...state.formData, [action.payload.field]: action.payload.value },
        errors: { ...state.errors, [action.payload.field]: undefined },
      };
    case 'SET_ERROR':
      return { ...state, errors: { ...state.errors, ...action.payload } };
    case 'NEXT_STEP':
      return { ...state, currentStep: state.currentStep + 1, errors: {} };
    case 'PREV_STEP':
      return { ...state, currentStep: state.currentStep - 1, errors: {} };
    case 'SUBMIT_START':
      return { ...state, isSubmitting: true };
    case 'SUBMIT_SUCCESS':
      return { ...initialState };
    default:
      return state;
  }
};
