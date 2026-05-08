// src/components/RegistrationForm/index.tsx
import React, { useReducer } from 'react';
import { Step1Schema, Step2Schema, Step3Schema } from './schema';
import { registrationReducer, initialState } from './reducer';
import type { IFormData } from './schema';
import type { ZodSchema } from 'zod';
import { useAuth } from '../../contexts/AuthContext';

const RegistrationForm: React.FC = () => {
  const [state, dispatch] = useReducer(registrationReducer, initialState);
  const { login } = useAuth();

  const { currentStep, formData, errors, isSubmitting } = state;

  const handleChange = (field: keyof IFormData, value: string | boolean) => {
    dispatch({ type: 'UPDATE_FIELD', payload: { field, value } });
  };

  const validateStep = (schema: ZodSchema): boolean => {
    const result = schema.safeParse(formData);
    if (!result.success) {
      const stepErrors: Partial<Record<keyof IFormData, string>> = {};
      result.error.issues.forEach((issue) => {
        const fieldName = issue.path[0] as keyof IFormData;
        stepErrors[fieldName] = issue.message;
      });
      dispatch({ type: 'SET_ERROR', payload: stepErrors });
      return false;
    }
    return true;
  };

  const handleNext = () => {
    let isValid = false;
    if (currentStep === 1) isValid = validateStep(Step1Schema);
    if (currentStep === 2) isValid = validateStep(Step2Schema);
    if (isValid) dispatch({ type: 'NEXT_STEP' });
  };

  const handlePrev = () => {
    dispatch({ type: 'PREV_STEP' });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const isValid = validateStep(Step3Schema);
    if (!isValid) return;

    dispatch({ type: 'SUBMIT_START' });

    setTimeout(() => {
      // Сохраняем пользователя в AuthContext
      login({
        email: formData.email ?? '',
        username: formData.username ?? '',
      });
      dispatch({ type: 'SUBMIT_SUCCESS' });
    }, 500);
  };

  const renderStep1 = () => (
    <div>
      <h3 style={styles.stepTitle}>Шаг 1: Аккаунт</h3>
      <div style={styles.inputGroup}>
        <label style={styles.label}>Email:</label>
        <input
          style={styles.input}
          type="email"
          value={formData.email ?? ''}
          onChange={(e) => handleChange('email', e.target.value)}
        />
        {errors.email && <span style={styles.error}>{errors.email}</span>}
      </div>
      <div style={styles.inputGroup}>
        <label style={styles.label}>Пароль:</label>
        <input
          style={styles.input}
          type="password"
          value={formData.password ?? ''}
          onChange={(e) => handleChange('password', e.target.value)}
        />
        {errors.password && <span style={styles.error}>{errors.password}</span>}
      </div>
    </div>
  );

  const renderStep2 = () => (
    <div>
      <h3 style={styles.stepTitle}>Шаг 2: Профиль</h3>
      <div style={styles.inputGroup}>
        <label style={styles.label}>Имя пользователя:</label>
        <input
          style={styles.input}
          type="text"
          value={formData.username ?? ''}
          onChange={(e) => handleChange('username', e.target.value)}
        />
        {errors.username && <span style={styles.error}>{errors.username}</span>}
      </div>
      <div style={styles.inputGroup}>
        <label style={styles.label}>Город:</label>
        <input
          style={styles.input}
          type="text"
          value={formData.city ?? ''}
          onChange={(e) => handleChange('city', e.target.value)}
        />
        {errors.city && <span style={styles.error}>{errors.city}</span>}
      </div>
    </div>
  );

  const renderStep3 = () => (
    <div>
      <h3 style={styles.stepTitle}>Шаг 3: О себе</h3>
      <div style={styles.inputGroup}>
        <label style={styles.label}>Профессия:</label>
        <select
          style={styles.input}
          value={formData.occupation ?? 'student'}
          onChange={(e) => handleChange('occupation', e.target.value)}
        >
          <option value="student">Студент</option>
          <option value="developer">Разработчик</option>
          <option value="designer">Дизайнер</option>
          <option value="other">Другое</option>
        </select>
        {errors.occupation && <span style={styles.error}>{errors.occupation}</span>}
      </div>
      <div style={styles.inputGroup}>
        <label style={{ ...styles.label, flexDirection: 'row', gap: '8px', alignItems: 'center' }}>
          <input
            type="checkbox"
            checked={!!formData.terms}
            onChange={(e) => handleChange('terms', e.target.checked)}
          />
          Согласен с правилами
        </label>
        {errors.terms && <span style={styles.error}>{errors.terms}</span>}
      </div>
    </div>
  );

  return (
    <div style={styles.container}>
      <h2 style={styles.title}>Регистрация (Шаг {currentStep} из 3)</h2>
      <form onSubmit={handleSubmit}>
        {currentStep === 1 && renderStep1()}
        {currentStep === 2 && renderStep2()}
        {currentStep === 3 && renderStep3()}
        <div style={styles.buttons}>
          {currentStep > 1 && (
            <button style={styles.btnSecondary} type="button" onClick={handlePrev} disabled={isSubmitting}>
              Назад
            </button>
          )}
          {currentStep < 3 ? (
            <button style={styles.btnPrimary} type="button" onClick={handleNext}>
              Далее
            </button>
          ) : (
            <button style={styles.btnPrimary} type="submit" disabled={isSubmitting}>
              {isSubmitting ? 'Отправка...' : 'Зарегистрироваться'}
            </button>
          )}
        </div>
      </form>
    </div>
  );
};

const styles: Record<string, React.CSSProperties> = {
  container: {
    maxWidth: '420px',
    margin: '60px auto',
    padding: '32px',
    border: '1px solid #e0e0e0',
    borderRadius: '12px',
    fontFamily: 'sans-serif',
    background: '#fff',
    boxShadow: '0 4px 24px rgba(0,0,0,0.08)',
  },
  title: { margin: '0 0 24px', fontSize: '20px', textAlign: 'center', color: '#1a1a1a' },
  stepTitle: { margin: '0 0 16px', fontSize: '16px', color: '#444' },
  inputGroup: { display: 'flex', flexDirection: 'column', marginBottom: '16px' },
  label: { display: 'flex', flexDirection: 'column', fontSize: '14px', color: '#555', marginBottom: '4px' },
  input: { padding: '8px 12px', border: '1px solid #ccc', borderRadius: '6px', fontSize: '14px' },
  error: { color: '#e53935', fontSize: '12px', marginTop: '4px' },
  buttons: { display: 'flex', justifyContent: 'space-between', marginTop: '24px', gap: '8px' },
  btnPrimary: { flex: 1, padding: '10px', background: '#1976d2', color: '#fff', border: 'none', borderRadius: '6px', cursor: 'pointer', fontSize: '14px' },
  btnSecondary: { flex: 1, padding: '10px', background: '#f5f5f5', color: '#333', border: '1px solid #ccc', borderRadius: '6px', cursor: 'pointer', fontSize: '14px' },
};

export default RegistrationForm;
