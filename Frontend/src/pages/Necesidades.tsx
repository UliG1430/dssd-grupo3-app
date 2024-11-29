import React, { useEffect, useState } from 'react';
import { getNecesidades, getStockMaterial } from '../service/apiService';

interface Necesidad {
  deposit: string;
  material: string;
  CodMaterial: string; // Asegúrate de que la necesidad tenga CodMaterial
  quantity: number;
}

const Necesidades: React.FC = () => {
  const [necesidades, setNecesidades] = useState<Necesidad[]>([]); // Inicializado como array vacío
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [modalData, setModalData] = useState<{
    necesidad: Necesidad | null;
    stockActual: number | null;
  }>({ necesidad: null, stockActual: null });

  useEffect(() => {
    const fetchNecesidades = async () => {
      try {
        const response = await getNecesidades();
        console.log('Respuesta de la API:', response);

        if (Array.isArray(response)) {
          setNecesidades(response); // Usar directamente el arreglo
        } else {
          console.error('Formato de datos no válido:', response);
          throw new Error('La respuesta no tiene el formato esperado.');
        }
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
      const stockResponse = await getStockMaterial(necesidad.CodMaterial);
      setModalData({
        necesidad,
        stockActual: stockResponse.StockActual,
      });
    } catch (error) {
      console.error('Error al verificar existencias:', error);
      setModalData({
        necesidad,
        stockActual: null,
      });
    }
  };

  const handleCerrarModal = () => {
    setModalData({ necesidad: null, stockActual: null });
  };

  const handleTomarPedido = (necesidad: Necesidad) => {
    console.log(`Pedido tomado para el depósito: ${necesidad.deposit}, material: ${necesidad.material}`);
    // Implementar lógica adicional aquí
    handleCerrarModal(); // Cierra el modal después de tomar el pedido
  };

  if (loading) return <p className="text-center">Cargando necesidades...</p>;
  if (error) return <p className="text-center text-red-500">{error}</p>;

  return (
    <div className="min-h-screen bg-gray-100 p-8">
      <div className="container mx-auto">
        <h1 className="text-3xl font-bold text-center text-gray-800 mb-6">
          Necesidades de los Depósitos
        </h1>
        {Array.isArray(necesidades) && necesidades.length > 0 ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {necesidades.map((necesidad, index) => (
              <div
                key={index}
                className="bg-white p-4 rounded-md shadow-md border border-gray-200"
              >
                <h2 className="text-xl font-semibold text-gray-700">{necesidad.deposit}</h2>
                <p className="mt-2 text-gray-600">Material: {necesidad.material}</p>
                <p className="mt-2 text-gray-600">Cantidad necesitada: {necesidad.quantity}kg</p>
                <button
                  className="mt-4 w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600"
                  onClick={() => handleVerificarExistencias(necesidad)} // Verificar existencias
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

      {/* Modal */}
      {modalData.necesidad && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white p-6 rounded-md shadow-lg w-96">
            <h2 className="text-2xl font-bold mb-4">Detalle de la Necesidad</h2>
            <p><strong>Depósito:</strong> {modalData.necesidad.deposit}</p>
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
        modalData.necesidad !== null &&
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
    </div>
  );
};

export default Necesidades;