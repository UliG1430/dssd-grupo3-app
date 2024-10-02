import React from 'react';
import { useForm } from 'react-hook-form';
import { enviarRecoleccion, RecoleccionData } from '../service/recoleccionService';

const PlanForm: React.FC = () => {
  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<RecoleccionData>();

  const onSubmit = async (data: RecoleccionData) => {
    try {
      await enviarRecoleccion(data);
      alert('Recolección enviada exitosamente');
      reset();
    } catch (error) {
      alert('Hubo un problema al enviar la recolección');
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6"> {/* Ajustamos el espacio entre los campos */}
      <div>
        <label className="block text-sm font-medium text-gray-700">Material</label>
        <input
          type="text"
          {...register('material', { required: 'El material es obligatorio' })}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
          placeholder="Ingrese el material"
        />
        {errors.material && <span className="text-red-500 text-sm">{errors.material.message}</span>}
      </div>

      <div>
        <label className="block text-sm font-medium text-gray-700">Cantidad (en kg)</label>
        <input
          type="number"
          {...register('quantity', { required: 'La cantidad es obligatoria', min: 0.1 })}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
          placeholder="Ingrese la cantidad"
        />
        {errors.quantity && <span className="text-red-500 text-sm">{errors.quantity.message}</span>}
      </div>

      <div>
        <label className="block text-sm font-medium text-gray-700">Punto</label>
        <select
          {...register('zone', { required: 'El punto es obligatorio' })}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
        >
          <option value="" disabled>Selecciona un punto</option>
          <option value="puntoA">Punto A</option>
          <option value="puntoB">Punto B</option>
          <option value="puntoC">Punto C</option>
          <option value="puntoD">Punto D</option>
        </select>
        {errors.zone && <span className="text-red-500 text-sm">{errors.zone.message}</span>}
      </div>

      <button
        type="submit"
        className="w-full bg-green-600 text-white p-2 rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
        disabled={isSubmitting}
      >
        {isSubmitting ? 'Enviando...' : 'Cargar recolección'}
      </button>
    </form>
  );
};

export default PlanForm;
