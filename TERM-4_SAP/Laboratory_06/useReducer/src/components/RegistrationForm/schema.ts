import { z } from 'zod';

export const RegistrationSchema = z.object({
  email: z
    .string()
    .regex(/^[^\s@]+@[^\s@]+\.[^\s@]+$/, "Введите корректный email адрес"),
  password: z
    .string()
    .min(8, "Пароль должен содержать минимум 8 символов"),
  username: z
    .string()
    .min(1, "Имя пользователя не может быть пустым"),
  city: z
    .string()
    .min(1, "Город не может быть пустым"),
  occupation: z.enum(["student", "developer", "designer", "other"], {
    message: "Пожалуйста, выберите профессию",
  }),
  terms: z.literal(true, {
    message: "Вы должны согласиться с правилами",
  }),
});

export type IFormData = z.infer<typeof RegistrationSchema>;

export const Step1Schema = RegistrationSchema.pick({ email: true, password: true });
export const Step2Schema = RegistrationSchema.pick({ username: true, city: true });
export const Step3Schema = RegistrationSchema.pick({ occupation: true, terms: true });