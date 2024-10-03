import React, { useState } from 'react';

interface ZonaSelectorProps {
  onZonaSeleccionada: (zona: string) => void;
}

const ZonaSelector: React.FC<ZonaSelectorProps> = ({ onZonaSeleccionada }) => {
  const [zona, setZona] = useState<string | null>(null);

  const handleZonaChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setZona(e.target.value);
  };

  const handleConfirmarZona = () => {
    if (zona) {
      onZonaSeleccionada(zona); // Llama a la función que propaga la zona seleccionada al componente padre
    } else {
      alert('Por favor selecciona una zona.');
    }
  };

  return (
    <div className="space-y-4">
      <h1 className="text-3xl font-bold text-green-800 mb-4">Selecciona una zona</h1>
      <select
        onChange={handleZonaChange}
        className="w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
      >
        <option value="" disabled>Selecciona una zona</option>
        <option value="puntoA">Punto A</option>
        <option value="puntoB">Punto B</option>
        <option value="puntoC">Punto C</option>
        <option value="puntoD">Punto D</option>
      </select>
      <button
        onClick={handleConfirmarZona}
        className="bg-green-600 text-white p-4 rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
      >
        Confirmar Zona
      </button>
    </div>
  );
};

export default ZonaSelector;
