import React from 'react';
import { useForm, SubmitHandler, FieldValues, FieldError } from 'react-hook-form';
import InputField from './InputField';
import Button from './Button';

export interface FormField {
  name: string;
  type: string;
  label: string;
  placeholder?: string;
  validation?: any;
  options?: { value: string | number; label: string }[];
}

interface GenericFormProps {
  fields: FormField[];
  onSubmit: SubmitHandler<FieldValues>;
  submitButtonText: string;
  isSubmitting: boolean;
}

const GenericForm: React.FC<GenericFormProps> = ({ fields, onSubmit, submitButtonText, isSubmitting }) => {
  const { register, handleSubmit, formState: { errors } } = useForm();

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6 w-full">
      {fields.map((field) => (
        field.type === 'select' ? (
          <div key={field.name}>
            <label className="block text-sm font-medium text-gray-700">{field.label}</label>
            <select
              {...register(field.name, {
                ...field.validation,
                validate: value => value !== '' || 'Debe seleccionar una opción',
              })}
              className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
            >
              <option value="">Seleccionar...</option>
              {field.options?.map(option => (
                <option key={option.value} value={option.value}>{option.label}</option>
              ))}
            </select>
            {/* Fix for type issue */}
            {errors[field.name] && (
              <span className="text-red-500 text-sm">
                {errors[field.name]?.message?.toString()}
              </span>
            )}
          </div>
        ) : (
          <InputField
            key={field.name}
            type={field.type}
            label={field.label}
            placeholder={field.placeholder}
            register={register}
            name={field.name}
            error={errors[field.name]}
            validation={field.validation}
          />
          
        )
      ))}
      <Button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Cargando...' : submitButtonText}
      </Button>
    </form>
  );
};

export default GenericForm;
