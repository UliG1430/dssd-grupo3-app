// src/components/InputField.tsx
import React from 'react';
import { FieldError, Merge, FieldErrorsImpl } from 'react-hook-form';

interface InputFieldProps {
  type: string;
  label: string;
  placeholder?: string;
  register: any;
  name: string;
  error?: FieldError | Merge<FieldError, FieldErrorsImpl<any>>;
  validation?: any;
}

const InputField: React.FC<InputFieldProps> = ({ type, label, placeholder, register, name, error, validation }) => (
  <div>
    <label className="block text-sm font-medium text-gray-700">{label}</label>
    <input
      type={type}
      {...register(name, validation)}
      className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
      placeholder={placeholder}
    />
    {error && <span className="text-red-500 text-sm">{(error as FieldError).message}</span>}
  </div>
);

export default InputField;