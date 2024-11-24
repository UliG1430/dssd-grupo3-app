import React, { useState, useEffect } from 'react';
import { SubmitHandler, FieldValues, set } from 'react-hook-form';
import GenericForm, { FormField } from '../components/Form';
import Modal from '../components/Modal';
import Button from '../components/Button';
import { getMateriales, addOrden } from '../service/recoleccionService';
import { executeTask, getTaskById, assignTask, setCaseVariable, getNextTaskId } from '../service/bonitaService';
import { useNavigate } from 'react-router-dom';

interface RecoleccionData {
  material: string;
  cantidad: number;
}

const CargarRecoleccion: React.FC = () => {
  const [error, setError] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const navigate = useNavigate();
  const [data, setData] = useState<RecoleccionData>({ material: '', cantidad: 0 });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [materiales, setMateriales] = useState<{ value: string | number; label: string }[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {

        const mats = await getMateriales();
        setMateriales(mats.map(m => ({ value: m.id, label: m.nombre })));
      } catch (error) {
        console.error('Error fetching data:', error);
        setError('Error fetching data');
      }
    };

    fetchData();
  }, []);

  const handleComenzarRecoleccion: SubmitHandler<FieldValues> = async (data) => {
    setIsSubmitting(true);
    try {
      setData(data as RecoleccionData);
      setError(null);
      console.log('Datos del formulario:', data);
      setIsModalOpen(true); // Open the modal
    } catch (error) {
      console.error('Error al procesar el formulario:', error);
      setError('Error al procesar el formulario');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
  };

  const handleKeepRecolecting = async () => {
    setIsModalOpen(false);
    await setCaseVariable(localStorage.getItem('caseId')!, 'pointsLeft', true);
    await proceedToNextTask();
  };

  const handleFinishRecolecting = async () => {
    setIsModalOpen(false);
    await setCaseVariable(localStorage.getItem('caseId')!, 'pointsLeft', false);
    await proceedToNextTask();
  };

  const proceedToNextTask = async () => {
    await addOrden({
      'Material': data.material,
      'Zona': localStorage.getItem('puntoRecoleccionId'),
      'Cantidad': data.cantidad,
      'UsuarioId': localStorage.getItem('idUser'),
      'CaseId': localStorage.getItem('caseId'),
    }); // Add the order
    try {
      const caseId = localStorage.getItem('caseId');
      let nextTaskId = await getNextTaskId(caseId!);
      await executeTask(nextTaskId);
      
      await new Promise(resolve => setTimeout(resolve, 2000));
      nextTaskId = await getNextTaskId(caseId!);

      const taskInfo = await getTaskById(nextTaskId);
      const bonitaUserId = localStorage.getItem('idUserBonita');
      if (bonitaUserId) {
        await assignTask(taskInfo.id, bonitaUserId);
        localStorage.setItem('nextTaskId', taskInfo.id);
      }

      if (taskInfo.name === 'Visitar punto') {
        navigate('/visitar-punto');
      } else {
        navigate('/esperar-cobro');
      }
    } catch (error) {
      console.error('Error al procesar la siguiente tarea:', error);
      setError('Error al procesar la siguiente tarea');
    }
  };

  const formFields: FormField[] = [
    {
      name: 'material',
      type: 'select',
      label: 'Material',
      options: materiales,
      validation: { required: 'El material es obligatorio' },
    },
    {
      name: 'cantidad',
      type: 'number',
      label: 'Cantidad',
      placeholder: 'Ingrese la cantidad',
      validation: { required: 'La cantidad es obligatoria', min: { value: 1, message: 'La cantidad debe ser al menos 1' } },
    },
  ];

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-green-50 p-6">
      <div className="bg-white p-8 rounded-md shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center">Cargar Recolección</h2>
        {error && <p className="text-red-500">{error}</p>}
        <GenericForm
          fields={formFields}
          onSubmit={handleComenzarRecoleccion}
          submitButtonText="Realizar Orden"
          isSubmitting={isSubmitting}
        />
      </div>

      <Modal isOpen={isModalOpen} onClose={handleModalClose} title="¿Qué desea hacer?">
        <Button onClick={handleKeepRecolecting} color="green">
          Seguir recolectando
        </Button>
        <Button onClick={handleFinishRecolecting} color="red">
          Terminar recolección
        </Button>
      </Modal>
    </div>
  );
};

export default CargarRecoleccion;