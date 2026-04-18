import React, { useReducer } from 'react';
import { Step1Schema, Step2Schema, Step3Schema } from './schema';
import { registrationReducer, initialState } from './reducer';
import type { IFormData } from './schema';
import type { ZodSchema } from 'zod';

export const RegistrationForm: React.FC = () => {
  const [state, dispatch] = useReducer(registrationReducer, initialState);

  const { currentStep, formData, errors, isSubmitting } = state;

  const handleChange = (field: keyof IFormData, value: string | boolean) => {
    dispatch({ type: 'UPDATE_FIELD', payload: { field, value } });
  };

  const validateStep = (schema: ZodSchema): boolean => {
    const result = schema.safeParse(formData);

    if (!result.success) {
      const stepErrors: Record<string, string> = {};
      result.error.issues.forEach((issue) => {
        const fieldName = issue.path[0] as string;
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

    if (isValid) {
      dispatch({ type: 'NEXT_STEP' });
    }
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
      console.log('Данные успешно отправлены:', formData);
      alert('Регистрация успешно завершена! Данные в консоли.');
      dispatch({ type: 'SUBMIT_SUCCESS' });
    }, 2000);
  };

  const renderStep1 = () => (
    <div>
      <h3>Шаг 1: Аккаунт</h3>
      <div style={styles.inputGroup}>
        <label>Email:</label>
        <input
          type="email"
          value={formData.email || ''}
          onChange={(e) => handleChange('email', e.target.value)}
        />
        {errors.email && <span style={styles.error}>{errors.email}</span>}
      </div>

      <div style={styles.inputGroup}>
        <label>Пароль:</label>
        <input
          type="password"
          value={formData.password || ''}
          onChange={(e) => handleChange('password', e.target.value)}
        />
        {errors.password && <span style={styles.error}>{errors.password}</span>}
      </div>
    </div>
  );

  const renderStep2 = () => (
    <div>
      <h3>Шаг 2: Профиль</h3>
      <div style={styles.inputGroup}>
        <label>Имя пользователя:</label>
        <input
          type="text"
          value={formData.username || ''}
          onChange={(e) => handleChange('username', e.target.value)}
        />
        {errors.username && <span style={styles.error}>{errors.username}</span>}
      </div>

      <div style={styles.inputGroup}>
        <label>Город:</label>
        <input
          type="text"
          value={formData.city || ''}
          onChange={(e) => handleChange('city', e.target.value)}
        />
        {errors.city && <span style={styles.error}>{errors.city}</span>}
      </div>
    </div>
  );

  const renderStep3 = () => (
    <div>
      <h3>Шаг 3: О себе</h3>
      <div style={styles.inputGroup}>
        <label>Профессия:</label>
        <select
          value={formData.occupation || 'student'}
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
        <label>
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
      <h2>Регистрация (Шаг {currentStep} из 3)</h2>
      <form onSubmit={handleSubmit}>
        
        {currentStep === 1 && renderStep1()}
        {currentStep === 2 && renderStep2()}
        {currentStep === 3 && renderStep3()}

        <div style={styles.buttons}>
          {currentStep > 1 && (
            <button type="button" onClick={handlePrev} disabled={isSubmitting}>
              Назад
            </button>
          )}

          {currentStep < 3 ? (
            <button type="button" onClick={handleNext}>
              Далее
            </button>
          ) : (
            <button type="submit" disabled={isSubmitting}>
              {isSubmitting ? 'Отправка...' : 'Зарегистрироваться'}
            </button>
          )}
        </div>
      </form>
    </div>
  );
};

const styles: Record<string, React.CSSProperties> = {
  container: { maxWidth: '400px', margin: '40px auto', padding: '20px', border: '1px solid #ccc', borderRadius: '8px', fontFamily: 'sans-serif' },
  inputGroup: { display: 'flex', flexDirection: 'column', marginBottom: '15px' },
  error: { color: 'red', fontSize: '12px', marginTop: '4px' },
  buttons: { display: 'flex', justifyContent: 'space-between', marginTop: '20px' },
};

export default RegistrationForm;