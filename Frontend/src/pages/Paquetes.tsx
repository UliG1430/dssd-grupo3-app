import React, { useState, useEffect } from 'react';
import { getPaquetesByState } from '../service/paquetesService';
import { useNavigate } from 'react-router-dom';
import Button from '../components/Button';
import { getNextTaskId, getUsuarioIdByUsername, getTaskById, executeTask, assignTask } from '../service/bonitaService';
import { get } from 'react-hook-form';
import { updateUsuarioById } from '../service/UsuarioService';

interface Paquete {
  id: number;
  caseId: number;
  estado: string;
}

const Paquetes: React.FC = () => {
  const [paquetes, setPaquetes] = useState<Paquete[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [hayPaquetes, setHayPaquetes] = useState<boolean>(false);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchPaquetes = async () => {
      try {
        const paquetesData = await getPaquetesByState('ENV');
        if (paquetesData.length === 0) {
          setHayPaquetes(false);
        } else {
            setHayPaquetes(true);
            setPaquetes(paquetesData);
        }
      } catch (error) {
        console.error('Error fetching paquetes:', error);
        setError('Ocurrió un error al recuperar los paquetes paquetes');
      }
    };

    fetchPaquetes();
  }, []);

  const handleAnalizarPaquete = async (paquete: Paquete) => {
      
      const caseId = paquete.caseId.toString();    
      localStorage.setItem('caseId', caseId);
      let nextTaskId = await getNextTaskId(caseId);
      localStorage.setItem('nextTaskId', nextTaskId);
      const taskInfo = await getTaskById(nextTaskId);
    
    if (taskInfo.name === 'Recibir paquetes') {
        await executeTask(nextTaskId);
        await new Promise(resolve => setTimeout(resolve, 2000));
        nextTaskId = await getNextTaskId(caseId);
        await assignTask(nextTaskId, localStorage.getItem('idUserBonita')!);
        localStorage.setItem('nextTaskId', nextTaskId);
        await updateUsuarioById(Number(localStorage.getItem('idUser')!), 1, false, 'R', true, Number(caseId));
    } else {
        nextTaskId = await getNextTaskId(caseId);
        localStorage.setItem('nextTaskId', nextTaskId);
    }

    if (taskInfo.name === 'Registrar resultado') {
        navigate(`/registrar-resultado/${caseId}`);
    } else
        navigate(`/analizar-ordenes/${paquete.id}`);
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-100 p-6">
      <h2 className="text-2xl font-bold mb-6">Paquetes para Analizar</h2>
      {error && (
        <div className="bg-red-500 text-white p-4 rounded-md mb-6">
          {error}
        </div>
      )}
      {!hayPaquetes && (
        <div className="bg-yellow-500 text-white p-4 rounded-md mb-6">
          No hay paquetes para analizar
        </div>
      )}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {hayPaquetes && paquetes.length > 0 && (
            paquetes.map(paquete => (
                <div key={paquete.id} className="bg-white p-6 rounded-md shadow-md">
                    <h3 className="text-xl font-bold mb-2">Caso: {paquete.caseId}</h3>
                    <Button onClick={() => handleAnalizarPaquete(paquete)} color="blue">
                        Analizar Paquete
                    </Button>
                </div>
            ))
        )}
      </div>
    </div>
  );
};

export default Paquetes;