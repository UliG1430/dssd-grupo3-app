import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../components/Button';
import { Box, Typography } from '@mui/material';
import { getNotificacionPagoByCaseId } from '../service/notificacionPagoService';
import { updateUsuarioById } from '../service/UsuarioService';
import { getNextTaskId, executeTask, setCaseVariable } from '../service/bonitaService';

const EsperarCobro: React.FC = () => {
  const [notificacionPago, setNotificacionPago] = useState<{ cantidad: number } | null>(null);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchNotificacionPago = async () => {
      try {
        const caseId = localStorage.getItem('caseId');
        if (caseId) {
          const notificacion = await getNotificacionPagoByCaseId(caseId);
          if (notificacion) {
            setNotificacionPago(notificacion);
          }
        }
      } catch (error) {
        console.error('Error fetching notificacion pago:', error);
        setError('Error fetching notificacion pago');
      }
    };

    fetchNotificacionPago();
  }, []);

  const handleConfirmarNotificacion = async () => {
    try {
      const caseId = localStorage.getItem('caseId');
      if (caseId) {
        const nextTaskId = await getNextTaskId(caseId);
        await setCaseVariable(caseId, 'evaluationDone', true);
        await executeTask(nextTaskId);

        const userId = localStorage.getItem('idUser');
        if (userId) {
          await updateUsuarioById(Number(userId), 0, false, 'R', false, 0);
        }

        navigate('/comenzar-recoleccion');
      }
    } catch (error) {
      console.error('Error confirming payment notification:', error);
      setError('Error confirming payment notification');
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-100 p-6">
      
      {error && <p className="text-red-500">{error}</p>}
      {notificacionPago ? (
        <div className="bg-white p-8 rounded-md shadow-md w-full max-w-md text-center">
          <p className="mb-4">Cantidad: {notificacionPago.cantidad}</p>
          <Button onClick={handleConfirmarNotificacion} color="blue">
            Confirmo haber recibido la notificación del pago
          </Button>
        </div>
      ) : (
        <Box
            display="flex"
            flexDirection="column"
            justifyContent="center"
            alignItems="center"
            height="100vh"
            textAlign="center"
        >
            <Typography variant="h4" gutterBottom>
                Espere a recibir la notificación de cobro
            </Typography>
            <Typography variant="subtitle1" color="textSecondary">
                Esto puede tardar algunos días
            </Typography>
        </Box>
      )}
    </div>
  );
};

export default EsperarCobro;