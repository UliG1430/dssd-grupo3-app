import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Button from '../components/Button';
import Modal from '../components/Modal';
import { getOrdenesByPaqueteId, updateOrdenState } from '../service/recoleccionService';
import { executeTask, getNextTaskId, assignTask, setCaseVariable } from '../service/bonitaService';
import { updatePaquete } from '../service/paquetesService';

interface Orden {
  id: number;
  material: string;
  puntoRecoleccion: { nombre: string };
  pesoKg: number;
  fecha: string;
}

const AnalizarOrdenes: React.FC = () => {
  const { paqueteId } = useParams<{ paqueteId: string }>();
  const [ordenes, setOrdenes] = useState<Orden[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedOrden, setSelectedOrden] = useState<Orden | null>(null);
  const [realPeso, setRealPeso] = useState<number | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchOrdenes = async () => {
      try {
        const ordenesData = await getOrdenesByPaqueteId(Number(paqueteId));
        setOrdenes(ordenesData);
      } catch (error) {
        console.error('Error fetching ordenes:', error);
        setError('Error fetching ordenes');
      }
    };

    fetchOrdenes();
  }, [paqueteId]);

  const handleAnalizarOrden = async (ordenId: number) => {
    try {
      
      const nextTaskId = localStorage.getItem('nextTaskId');
      const bonitaUserId = localStorage.getItem('idUserBonita');
      await assignTask(nextTaskId!, bonitaUserId!);
      if (nextTaskId) {
        await executeTask(nextTaskId);
      }

      const caseId = localStorage.getItem('caseId');
      if (caseId) {
        const nextTask = await getNextTaskId(caseId);
        console.log('Next task:', nextTask);

        
        if (bonitaUserId) {
          await assignTask(nextTask, bonitaUserId);
          localStorage.setItem('nextTaskId', nextTask);
        }
      }

      const orden = ordenes.find(o => o.id === ordenId);
      setSelectedOrden(orden || null);
      setIsModalOpen(true);
    } catch (error) {
      console.error('Error al procesar la orden:', error);
      setError('Error al procesar la orden');
    }
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setSelectedOrden(null);
    setRealPeso(null);
  };

  const handleSubmit = async () => {
    if (selectedOrden && realPeso !== null) {
      const newState = selectedOrden.pesoKg === realPeso ? 'OK' : 'INV';
      await updateOrdenState(selectedOrden.id, newState);

      // Retrieve the orders again
      const ordenesData = await getOrdenesByPaqueteId(Number(paqueteId));
      if (ordenesData.length === 0) {
        await updatePaquete(Number(paqueteId), 'FIN');
        navigate('/realizar-notificacion-pago');
      }
      setOrdenes(ordenesData);

      // Set the Bonita variable packagesLeft
      const packagesLeft = ordenesData.length > 0;
      await setCaseVariable(localStorage.getItem('caseId')!, 'packagesLeft', packagesLeft);

      const nextTaskId = localStorage.getItem('nextTaskId');
      if (nextTaskId) {
        await executeTask(nextTaskId);
      }

      const caseId = localStorage.getItem('caseId');
      if (caseId) {
        const nextTask = await getNextTaskId(caseId);
        console.log('nextTaskId:', nextTask);

        const bonitaUserId = localStorage.getItem('idUserBonita');
        if (bonitaUserId) {
          await assignTask(nextTask, bonitaUserId);
          localStorage.setItem('nextTaskId', nextTask);
        }
      }
      if (!packagesLeft) 
        navigate('/realizar-notificacion-pago');

    }
}

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-100 p-6">
      <h2 className="text-2xl font-bold mb-6">Órdenes para Analizar</h2>
      {error && <p className="text-red-500">{error}</p>}
      <div className="space-y-6 w-full max-w-3xl">
        {ordenes.map(orden => (
          <div key={orden.id} className="bg-white p-6 rounded-md shadow-md">
            <h3 className="text-xl font-bold mb-2">Número de orden: {orden.id}</h3>
            <p className="mb-2"><strong>Material:</strong> {orden.material === '1' ? 'Ladrillo' : orden.material === '2' ? 'Plástico' : 'Aluminio'}</p>
            <p className="mb-2"><strong>Punto de Recolección:</strong> {orden.puntoRecoleccion.nombre}</p>
            <p className="mb-2"><strong>Cantidad:</strong> {orden.pesoKg}</p>
            <Button onClick={() => handleAnalizarOrden(orden.id)} color="blue">
              Analizar Orden
            </Button>
          </div>
        ))}
      </div>

      <Modal isOpen={isModalOpen} onClose={handleModalClose} title="Analizar Orden">
        {selectedOrden && (
          <div>
            <div className="mb-4">
              <label className="block text-sm font-medium text-gray-700">Peso indicado por el recolector</label>
              <input
                type="number"
                value={selectedOrden.pesoKg}
                readOnly
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
              />
            </div>
            <div className="mb-4">
              <label className="block text-sm font-medium text-gray-700">Peso real de la orden</label>
              <input
                type="number"
                value={realPeso || ''}
                onChange={(e) => setRealPeso(Number(e.target.value))}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
              />
            </div>
            <Button onClick={handleSubmit} color="blue">
              Confirmar
            </Button>
          </div>
        )}
      </Modal>
    </div>
  );
};

export default AnalizarOrdenes;