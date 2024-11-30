import React, { useEffect, useState } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import {
  getOrdenesDistribucion,
  tomarOrdenDistribucion,
  confirmarEntregaOrdenDistribucion,
} from '../service/apiService';

interface OrdenDistribucion {
  id: number;
  necesidad_id: number;
  estado: string;
}

const OrdenesDistribucion: React.FC = () => {
  const [ordenes, setOrdenes] = useState<OrdenDistribucion[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchOrdenes = async () => {
      try {
        const response = await getOrdenesDistribucion();
        setOrdenes(response);
        setLoading(false);
      } catch (err) {
        console.error('Error al cargar las órdenes de distribución:', err);
        setError('Error al cargar las órdenes. Por favor intenta de nuevo.');
        setLoading(false);
      }
    };

    fetchOrdenes();
  }, []);

  const handleTomarOrden = async (ordenId: number) => {
    try {
      await tomarOrdenDistribucion(ordenId);
      toast.success(`Orden ${ordenId} tomada exitosamente.`);
      setOrdenes((prev) =>
        prev.map((orden) =>
          orden.id === ordenId ? { ...orden, estado: 'en proceso' } : orden
        )
      );
    } catch (error) {
      console.error('Error al tomar la orden:', error);
      toast.error('Error al tomar la orden. Intenta de nuevo.');
    }
  };

  const handleConfirmarEntrega = async (ordenId: number) => {
    try {
      await confirmarEntregaOrdenDistribucion(ordenId);
      toast.success(`Orden ${ordenId} confirmada como entregada.`);
      setOrdenes((prev) =>
        prev.map((orden) =>
          orden.id === ordenId ? { ...orden, estado: 'entregada' } : orden
        )
      );
    } catch (error) {
      console.error('Error al confirmar la entrega:', error);
      toast.error('Error al confirmar la entrega. Intenta de nuevo.');
    }
  };

  if (loading) return <p className="text-center">Cargando órdenes...</p>;
  if (error) return <p className="text-center text-red-500">{error}</p>;

  return (
    <div className="min-h-screen bg-gray-100 p-8">
      <ToastContainer />
      <div className="container mx-auto">
        <h1 className="text-3xl font-bold text-center text-gray-800 mb-6">
          Órdenes de Distribución
        </h1>
        {ordenes.length > 0 ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {ordenes.map((orden) => (
              <div
                key={orden.id}
                className="bg-white p-4 rounded-md shadow-md border border-gray-200"
              >
                <h2 className="text-xl font-semibold text-gray-700">ID Orden: {orden.id}</h2>
                <p className="mt-2 text-gray-600">Necesidad ID: {orden.necesidad_id}</p>
                <p className="mt-2 text-gray-600">Estado: {orden.estado}</p>
                {orden.estado === 'pendiente' && (
                  <button
                    className="mt-4 w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600"
                    onClick={() => handleTomarOrden(orden.id)}
                  >
                    Tomar Orden
                  </button>
                )}
                {orden.estado === 'en proceso' && (
                  <button
                    className="mt-4 w-full bg-green-500 text-white py-2 px-4 rounded-md hover:bg-green-600"
                    onClick={() => handleConfirmarEntrega(orden.id)}
                  >
                    Confirmar Entrega
                  </button>
                )}
              </div>
            ))}
          </div>
        ) : (
          <p className="text-center text-gray-500">No hay órdenes disponibles.</p>
        )}
      </div>
    </div>
  );
};

export default OrdenesDistribucion;