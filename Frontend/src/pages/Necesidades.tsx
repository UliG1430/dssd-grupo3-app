import React, { useEffect, useState } from 'react';
import {
  getNecesidades,
  getStockMaterial,
  checkDepositoProveedor,
  addDepositoProveedor,
  tomarNecesidad,
  reduceMaterialStock,
} from '../service/apiService';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { executeTask, setCaseVariable } from '../service/bonitaService';

interface Necesidad {
  id: number;
  material: string;
  cod_material: string;
  quantity: number;
  deposito_id: number;
  material_id: number;
  estado: string;
}

const Necesidades: React.FC = () => {
  const [necesidades, setNecesidades] = useState<Necesidad[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [modalData, setModalData] = useState<{
    necesidad: Necesidad | null;
    stockActual: number | null;
    combinationExists: boolean | null;
  }>({ necesidad: null, stockActual: null, combinationExists: null });
  const [providerModalData, setProviderModalData] = useState<{
    necesidad: Necesidad | null;
  }>({ necesidad: null });
  const [registeringProvider, setRegisteringProvider] = useState(false);

  useEffect(() => {
    const fetchNecesidades = async () => {
      try {
        const response = await getNecesidades();
        const caseId=localStorage.getItem('caseId');
        if (caseId){
        if (response.length === 0) {
          await setCaseVariable(caseId,"hay_necesidades", false);

        }
        else
        {
          await setCaseVariable(caseId,"hay_necesidades", true);
        }
         await executeTask(localStorage.getItem('nextTaskId')!); 
      }


        console.log('Respuesta de getNecesidades:', response);
        setNecesidades(response);
        setLoading(false);
      } catch (err) {
        console.error('Error al cargar las necesidades:', err);
        setError('Error al cargar las necesidades. Por favor intenta de nuevo.');
        setLoading(false);
      }
    };

    fetchNecesidades();
  }, []);

  const handleVerificarExistencias = async (necesidad: Necesidad) => {
    try {
      console.log('Necesidad seleccionada:', necesidad);
      const stockResponse = await getStockMaterial(necesidad.cod_material);
      setModalData({
        necesidad,
        stockActual: stockResponse,
        combinationExists: null,
      });
    } catch (error) {
      console.error('Error al verificar existencias:', error);
      setModalData({
        necesidad,
        stockActual: null,
        combinationExists: null,
      });
    }
  };

  const handleTomarPedido = async (necesidad: Necesidad) => {
    try {
      const combinationExists = await checkDepositoProveedor(necesidad.material_id, necesidad.deposito_id);

      if (!combinationExists) {
        setProviderModalData({ necesidad });
        setModalData({ necesidad: null, stockActual: null, combinationExists: null });
        return;
      }

      // Cambiar el estado de la necesidad en el backend
      await tomarNecesidad(necesidad.id);

      // Reducir el stock en el backend
      await reduceMaterialStock(necesidad.cod_material, necesidad.quantity);

      toast.success(`La necesidad "${necesidad.id}" ha sido tomada y el stock del material "${necesidad.material}" ha sido reducido.`);

      // Actualizar la UI (eliminar la necesidad tomada)
      setNecesidades((prev) => prev.filter((n) => n.id !== necesidad.id));
      setModalData({ necesidad: null, stockActual: null, combinationExists: true });
    } catch (error) {
      console.error('Error al tomar el pedido:', error);
      toast.error('Error al tomar el pedido. Por favor intenta de nuevo.');
    }
  };

  const handleCerrarModal = () => {
    setModalData({ necesidad: null, stockActual: null, combinationExists: null });
  };

  const handleCerrarProviderModal = () => {
    setProviderModalData({ necesidad: null });
  };

  const handleRegistrarProveedor = async (necesidad: Necesidad) => {
    try {
      setRegisteringProvider(true);
      console.log('Iniciando registro de proveedor para necesidad:', necesidad);

      await addDepositoProveedor(necesidad.deposito_id, necesidad.material_id, necesidad.cod_material);

      toast.success(
        `El proveedor para el depósito "${necesidad.deposito_id}" y material "${necesidad.material}" ha sido registrado exitosamente.`
      );

      // Cerramos el modal de registro
      handleCerrarProviderModal();

      // Llamamos nuevamente a verificar existencias para abrir el modal de detalle con stock actualizado
      await handleVerificarExistencias(necesidad);
    } catch (error) {
      console.error('Error al registrar el proveedor:', error);
      toast.error('Error al registrar el proveedor. Por favor, inténtalo de nuevo.');
    } finally {
      setRegisteringProvider(false);
    }
  };

  if (loading) return <p className="text-center">Cargando necesidades...</p>;
  if (error) return <p className="text-center text-red-500">{error}</p>;

  return (
    <div className="min-h-screen bg-gray-100 p-8">
      <ToastContainer />
      <div className="container mx-auto">
        <h1 className="text-3xl font-bold text-center text-gray-800 mb-6">
          Necesidades de los Depósitos
        </h1>
        {necesidades.length > 0 ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {necesidades.map((necesidad) => (
              <div
                key={necesidad.id}
                className="bg-white p-4 rounded-md shadow-md border border-gray-200"
              >
                <h2 className="text-xl font-semibold text-gray-700">ID PEDIDO: {necesidad.id}</h2>
                <p className="mt-2 text-gray-600">Depósito: {necesidad.deposito_id}</p>
                <p className="mt-2 text-gray-600">Material: {necesidad.material}</p>
                <p className="mt-2 text-gray-600">Cantidad Necesitada: {necesidad.quantity}kg</p>
                <button
                  className="mt-4 w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600"
                  onClick={() => handleVerificarExistencias(necesidad)}
                >
                  Verificar Existencias
                </button>
              </div>
            ))}
          </div>
        ) : (
          <p className="text-center text-gray-500">No hay necesidades disponibles.</p>
        )}
      </div>

      {/* Modal de Detalle */}
      {modalData.necesidad && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white p-6 rounded-md shadow-lg w-96">
            <h2 className="text-2xl font-bold mb-4">Detalle de la Necesidad</h2>
            <p><strong>Depósito:</strong> {modalData.necesidad.deposito_id}</p>
            <p><strong>Material:</strong> {modalData.necesidad.material}</p>
            <p><strong>Stock Actual:</strong> {modalData.stockActual !== null ? `${modalData.stockActual}kg` : 'Error al obtener el stock'}</p>
            <p><strong>Cantidad Necesitada:</strong> {modalData.necesidad.quantity}kg</p>

            <div className="mt-4 flex justify-end space-x-4">
              <button
                className="bg-gray-500 text-white px-4 py-2 rounded-md hover:bg-gray-600"
                onClick={handleCerrarModal}
              >
                Cerrar
              </button>

              {modalData.stockActual !== null &&
                modalData.stockActual >= modalData.necesidad.quantity && (
                  <button
                    className="bg-green-500 text-white px-4 py-2 rounded-md hover:bg-green-600"
                    onClick={() => handleTomarPedido(modalData.necesidad!)}
                  >
                    Tomar Pedido
                  </button>
              )}
            </div>
          </div>
        </div>
      )}

      {/* Modal de Registrar Proveedor */}
      {providerModalData.necesidad && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white p-6 rounded-md shadow-lg w-96">
            <h2 className="text-2xl font-bold mb-4">Registrar Proveedor</h2>
            <p><strong>Depósito:</strong> {providerModalData.necesidad.deposito_id}</p>
            <p><strong>Material:</strong> {providerModalData.necesidad.material}</p>
            <p className="text-red-500 mt-2">
              Depósito {providerModalData.necesidad.deposito_id} no se encuentra registrado como proveedor del material {providerModalData.necesidad.material}.
            </p>

            <div className="mt-4 flex justify-end space-x-4">
              <button
                className="bg-gray-500 text-white px-4 py-2 rounded-md hover:bg-gray-600"
                onClick={handleCerrarProviderModal}
              >
                Cerrar
              </button>
              <button
                className={`bg-yellow-500 text-white px-4 py-2 rounded-md hover:bg-yellow-600 ${registeringProvider ? 'opacity-50 cursor-not-allowed' : ''}`}
                onClick={() => handleRegistrarProveedor(providerModalData.necesidad!)}
                disabled={registeringProvider}
              >
                {registeringProvider ? 'Registrando...' : 'Registrar Proveedor'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Necesidades;