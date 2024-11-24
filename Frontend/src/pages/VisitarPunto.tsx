import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { SubmitHandler, FieldValues, get } from 'react-hook-form';
import Form from '../components/Form';
import { getPuntosRecoleccion } from '../service/recoleccionService';
import { executeTask, getNextTaskId, assignTask } from '../service/bonitaService';

interface VisitarPuntoData {
  puntoRecoleccion: string;
}

export interface FormField {
  name: string;
  type: string;
  label: string;
  placeholder?: string;
  validation?: any;
  options?: { value: string | number; label: string }[];
}

const VisitarPunto: React.FC = () => {
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [puntosRecoleccion, setPuntosRecoleccion] = useState<{ value: string | number; label: string }[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const puntos = await getPuntosRecoleccion();
        setPuntosRecoleccion(puntos.map(p => ({ value: p.id, label: p.nombre })));
      } catch (error) {
        console.error('Error fetching data:', error);
        setError('Error fetching data');
      }
    };

    fetchData();
  }, []);

  const handleVisitarPunto: SubmitHandler<FieldValues> = async (data) => {
    setIsSubmitting(true);
    try {
      setError(null);
      console.log('Datos del formulario:', data);
    
      // Store puntoRecoleccionId in localStorage
      localStorage.setItem('puntoRecoleccionId', data.puntoRecoleccion);

      // Complete the current task
      const nextTaskId = await getNextTaskId(localStorage.getItem('caseId')!);
      await executeTask(nextTaskId);
      
      // Get the next task
      const caseId = localStorage.getItem('caseId');
      if (caseId) {
        const nextTask = await getNextTaskId(caseId);
        console.log('Next task:', nextTask);

        // Assign the next task to the Bonita user
        const bonitaUserId = localStorage.getItem('idUserBonita');
        if (bonitaUserId) {
          await assignTask(nextTask, bonitaUserId);
          localStorage.setItem('nextTaskId', nextTask);
        }
      }

      navigate('/cargar-recoleccion');
    } catch (error) {
      console.error('Error al procesar el formulario:', error);
      setError('Error al procesar el formulario');
    } finally {
      setIsSubmitting(false);
    }
  };

  const formFields: FormField[] = [
    {
      name: 'puntoRecoleccion',
      type: 'select',
      label: '',
      options: puntosRecoleccion,
      validation: { required: 'El punto de recolección es obligatorio' },
    },
  ];

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-green-50 p-6">
      <div className="bg-white p-8 rounded-md shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center">Punto de Recolección</h2>
        {error && <p className="text-red-500">{error}</p>}
        <Form
          fields={formFields}
          onSubmit={handleVisitarPunto}
          submitButtonText="Continuar"
          isSubmitting={isSubmitting}
        />
      </div>
    </div>
  );
};

export default VisitarPunto;