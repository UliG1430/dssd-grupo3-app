import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../components/Button';
import { getEvaluaciones, Evaluacion } from '../service/EvaluacionService';
import { getNextTaskId, assignTask, executeTask } from '../service/bonitaService';

const Evaluaciones: React.FC = () => {
  const [evaluaciones, setEvaluaciones] = useState<Evaluacion[]>([]);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchEvaluaciones = async () => {
      try {
        const evaluacionesData = await getEvaluaciones();
        const evaluacionesDataFiltered = evaluacionesData.filter((o: Evaluacion) => o.state === "ENV");
        if (evaluacionesData.length != 0) {
            setEvaluaciones(evaluacionesData);
        }
        
      } catch (error) {
        console.error('Error fetching evaluaciones:', error);
        setError('Error fetching evaluaciones');
      }
    };

    fetchEvaluaciones();
  }, []);

  const handleRealizarEvaluacion = async (caseId: number) => {
    try {
      const nextTaskId = await getNextTaskId(caseId.toString());
      localStorage.setItem('nextTaskId', nextTaskId);
      navigate('/realizar-evaluacion');
      
    } catch (error) {
      console.error('Error processing evaluation:', error);
      setError('Error processing evaluation');
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-100 p-6">
      <h2 className="text-2xl font-bold mb-6">Evaluaciones</h2>
      {error && <p className="text-red-500">{error}</p>}
      {evaluaciones.length === 0 ? (
        <p className="text-gray-700">No hay evaluaciones pendientes</p>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {evaluaciones.map(evaluacion => (
            <div key={evaluacion.caseId} className="bg-white p-6 rounded-md shadow-md">
              <p className="mb-2"><strong>Caso:</strong> {evaluacion.caseId}</p>
              <Button onClick={() => handleRealizarEvaluacion(evaluacion.caseId)} color="blue">
                Realizar evaluación
              </Button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Evaluaciones;