import React, { useState } from 'react';

interface ZonaSelectorProps {
  onZonaSeleccionada: (zona: string) => void;
  onProcessIdReceived: (processName: string) => Promise<void>;
}

const ZonaSelector: React.FC<ZonaSelectorProps> = ({ onZonaSeleccionada, onProcessIdReceived }) => {
  const [selectedZona, setSelectedZona] = useState<string | null>(null);

  const handleSeleccionarZona = () => {
    if (selectedZona) {
      onZonaSeleccionada(selectedZona);
      // Llamar a onProcessIdReceived pasando el nombre del proceso (puedes ajustar el nombre según tu necesidad)
      onProcessIdReceived('Proceso de recolección');
    }
  };

  return (
    <div>
      <h2>Selecciona una zona:</h2>
      <select
        value={selectedZona || ''}
        onChange={(e) => setSelectedZona(e.target.value)}
        className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
      >
        <option value="" disabled>Selecciona un punto</option>
        <option value="puntoA">Punto A</option>
        <option value="puntoB">Punto B</option>
        <option value="puntoC">Punto C</option>
        <option value="puntoD">Punto D</option>
      </select>
      <button
        onClick={handleSeleccionarZona}
        className="bg-green-600 text-white p-2 rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500 mt-4"
      >
        Seleccionar zona
      </button>
    </div>
  );
};

export default ZonaSelector;
