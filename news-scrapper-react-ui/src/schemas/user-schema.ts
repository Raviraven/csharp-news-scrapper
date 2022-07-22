import * as yup from 'yup';

export const UserLoginSchema = yup.object({
    username: yup.string().required(),
    password: yup.string().required(),
});

export type UserLogin = yup.InferType<typeof UserLoginSchema>;