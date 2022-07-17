import { HTMLInputTypeAttribute } from "react";
import MuiTextField from "@mui/material/TextField";
import { useField } from "formik";

interface MuiTextFieldProps {
  name: string;
  label: string;
  type?: HTMLInputTypeAttribute;
}

export const TextField = (props: MuiTextFieldProps) => {
  const [field, meta] = useField(props.name);

  const error: boolean = meta.touched && !!meta.error;
  const errorMessage: string | undefined = error ? meta.error : undefined;

  return (
    <MuiTextField
      error={error}
      name={field.name}
      value={field.value}
      onChange={field.onChange}
      onBlur={field.onBlur}
      fullWidth
      label={props.label}
      helperText={errorMessage}
      variant="standard"
      margin="dense"
      type={props.type}
    />
  );
};
