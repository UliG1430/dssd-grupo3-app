import React, { useState } from 'react';
import { addOrden } from '../service/recoleccionService'; // Método para agregar la orden

interface MaterialFormProps {
  zona: string;
}

const MaterialForm: React.FC<MaterialFormProps> = ({ zona }) => {
  const [material, setMaterial] = useState('');
  const [cantidad, setCantidad] = useState('');

  const handleCargarOrden = async () => {
    try {
      const data = {
        zona,
        material,
        cantidad: parseFloat(cantidad)
      };
      await addOrden(data);  // Llama al método AddOrden del backend
      alert('Orden cargada exitosamente');
    } catch (error) {
      console.error('Error al cargar la orden:', error);
    }
  };

  return (
    <div>
      <h2 className="text-2xl font-bold text-green-800 mb-4">Ingresar datos de recolección</h2>
      <div>
        <label className="block text-sm font-medium text-gray-700">Material</label>
        <input
          type="text"
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
          value={material}
          onChange={(e) => setMaterial(e.target.value)}
          placeholder="Ingrese el material"
        />
      </div>
      <div className="mt-4">
        <label className="block text-sm font-medium text-gray-700">Cantidad (kg)</label>
        <input
          type="number"
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
          value={cantidad}
          onChange={(e) => setCantidad(e.target.value)}
          placeholder="Ingrese la cantidad"
        />
      </div>
      <button
        className="mt-4 bg-green-600 text-white p-2 rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
        onClick={handleCargarOrden}
      >
        Cargar orden
      </button>
    </div>
  );
};

export default MaterialForm;
