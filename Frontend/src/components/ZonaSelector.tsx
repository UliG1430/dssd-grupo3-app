import React, { useState } from 'react';
import { startProcessById } from '../service/bonitaService'; // Importar el método

interface ZonaSelectorProps {
  onZonaSeleccionada: (zona: string) => void;
  onProcessIdReceived: (processName: string) => void; // Asegúrate de que esta función maneje processId
  processId: string;  // Recibe el processId después de seleccionarlo
  token: string;  // Recibe el token
}

const ZonaSelector: React.FC<ZonaSelectorProps> = ({ onZonaSeleccionada, onProcessIdReceived, processId, token }) => {
  const [selectedZona, setSelectedZona] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isInstancingProcess, setIsInstancingProcess] = useState(false); // Estado para evitar múltiples instancias
  const [processInstanceId, setProcessInstanceId] = useState<string | null>(null); // Guardar instancia del proceso

  const zonas = [
    { value: 'puntoA', label: 'Zona A' },
    { value: 'puntoB', label: 'Zona B' },
    { value: 'puntoC', label: 'Zona C' },
    { value: 'puntoD', label: 'Zona D' },
  ];

  const handleSeleccionarZona = async () => {
    if (!selectedZona) {
      setError('Por favor, selecciona una zona antes de continuar.');
      return;
    }

    try {
      setError(null); // Reseteamos cualquier error previo

      const zonaSeleccionada = zonas.find(zona => zona.value === selectedZona)?.label || '';
      onZonaSeleccionada(zonaSeleccionada);

      // Evitar múltiples llamados a la instancia del proceso
      if (!processInstanceId && !isInstancingProcess) {
        setIsInstancingProcess(true); // Bloquear el botón de instancia de proceso
        const processInstance = await startProcessById(processId, token);
        console.log('Instancia de proceso iniciada:', processInstance);

        if (processInstance?.id) {
          setProcessInstanceId(processInstance.id); // Guardamos la instancia para evitar duplicaciones
          onProcessIdReceived(processInstance.id);
        } else {
          throw new Error('No se pudo obtener el ID del caso');
        }
      }
    } catch (err) {
      setError('Error al iniciar el proceso. Intenta nuevamente.');
      console.error(err);
    } finally {
      setIsInstancingProcess(false); // Liberar el bloqueo del botón
    }
  };

  return (
    <div>
      <h2>Selecciona una zona:</h2>
      <select
        value={selectedZona || ''}
        onChange={(e) => setSelectedZona(e.target.value)}  // Guardar el valor (value) de la opción seleccionada
        className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
      >
        <option value="" disabled>Selecciona un punto</option>
        {zonas.map((zona) => (
          <option key={zona.value} value={zona.value}>
            {zona.label}
          </option>
        ))}
      </select>
      {error && <p className="text-red-500 mt-2">{error}</p>}
      <button
        onClick={handleSeleccionarZona}
        className="bg-green-600 text-white p-2 rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500 mt-4"
        disabled={isInstancingProcess} // Deshabilitamos el botón mientras se está iniciando el proceso
      >
        {isInstancingProcess ? 'Iniciando...' : 'Seleccionar zona'}
      </button>
    </div>
  );
};

export default ZonaSelector;
