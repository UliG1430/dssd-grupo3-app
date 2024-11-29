import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const HomeRedGlobal: React.FC = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('redGlobalToken');
    if (!token) {
      navigate('/login-red-global');
    }
  }, [navigate]);

  return (
    <div className="min-h-screen bg-blue-100">
      <div className="container mx-auto p-8">
        <h1 className="text-3xl font-bold text-center text-blue-800 mb-6">
          Bienvenido a la Red Global de Recicladores
        </h1>
        <div className="mt-6 text-center">
          <button
            onClick={() => navigate('/necesidades')}
            className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 transition"
          >
            Consultar necesidades de depósitos
          </button>
        </div>
      </div>
    </div>
  );
};

export default HomeRedGlobal;