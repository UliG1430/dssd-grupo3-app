import React from 'react';
import { useForm } from 'react-hook-form';
import { addOrden } from '../service/recoleccionService';  // Importamos el servicio

// Define el tipo de los datos de la orden (ajusta según lo que uses)
interface OrdenData {
  Material: string;
  Cantidad: number;
  Zona: string;
}

const PlanForm: React.FC = () => {
  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<OrdenData>();

  const onSubmit = async (data: OrdenData) => {
    try {
      // Llamamos al servicio para guardar la orden
      await addOrden(data);
      alert('Orden guardada exitosamente');
      reset(); // Reseteamos el formulario después de guardar
    } catch (error) {
      alert('Hubo un problema al guardar la orden');
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      <div>
        <label className="block text-sm font-medium text-gray-700">Material</label>
        <input
          type="text"
          {...register('Material', { required: 'El material es obligatorio' })}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
          placeholder="Ingrese el material"
        />
        {errors.Material && <span className="text-red-500 text-sm">{errors.Material.message}</span>}
      </div>

      <div>
        <label className="block text-sm font-medium text-gray-700">Cantidad (en kg)</label>
        <input
          type="number"
          {...register('Cantidad', { required: 'La cantidad es obligatoria', min: 0.1 })}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
          placeholder="Ingrese la cantidad"
        />
        {errors.Cantidad && <span className="text-red-500 text-sm">{errors.Cantidad.message}</span>}
      </div>

      <div>
        <label className="block text-sm font-medium text-gray-700">Punto</label>
        <select
          {...register('Zona', { required: 'El punto es obligatorio' })}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
        >
          <option value="" disabled>Selecciona un punto</option>
          <option value="puntoA">Punto A</option>
          <option value="puntoB">Punto B</option>
          <option value="puntoC">Punto C</option>
          <option value="puntoD">Punto D</option>
        </select>
        {errors.Zona && <span className="text-red-500 text-sm">{errors.Zona.message}</span>}
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
