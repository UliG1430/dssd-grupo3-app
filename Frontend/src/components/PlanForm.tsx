import React from 'react';
import { useForm } from 'react-hook-form';
import { addOrden } from '../service/recoleccionService';  // Importamos el servicio
import { completeTask } from '../service/bonitaService';  // Importamos el servicio para completar la tarea

interface OrdenData {
  Material: string;
  Cantidad: number;
  Zona: string;
}

interface PlanFormProps {
  zona: string;
  processInstance: string;  // Usamos solo processInstance en lugar de processId
  token: string;  // Añadimos el token
}

const PlanForm: React.FC<PlanFormProps> = ({ zona, processInstance, token }) => {
  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<OrdenData>();

  const onSubmit = async (data: OrdenData) => {
    try {
      // Llamamos al servicio para guardar la orden
      await addOrden(data);

      // Llamar al servicio para completar la tarea
      const resultado = await completeTask(processInstance, token);
      console.log('Resultado al completar la tarea:', resultado);

      alert('Orden guardada exitosamente y tarea completada');
      reset(); // Reseteamos el formulario después de guardar
    } catch (error) {
      alert('Hubo un problema al guardar la orden o completar la tarea');
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      {/* Material */}
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

      {/* Cantidad */}
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

      {/* Zona */}
      <div>
        <label className="block text-sm font-medium text-gray-700">Zona</label>
        <select
          {...register('Zona', { required: 'La zona es obligatoria' })}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
          defaultValue={zona}  // Asegúrate de que la zona seleccionada sea visible
        >
          <option value="Zona A">Zona A</option>
          <option value="Zona B">Zona B</option>
          <option value="Zona C">Zona C</option>
          <option value="Zona D">Zona D</option>
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
