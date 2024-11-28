// src/pages/HomeRedGlobal.tsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

const HomeRedGlobal: React.FC = () => {
  const [userData, setUserData] = useState<any>(null); // Guardar los datos del usuario
  const navigate = useNavigate();

  useEffect(() => {
    // Obtener el token JWT de localStorage
    const token = localStorage.getItem('redGlobalToken');
    if (!token) {
      // Si no hay token, redirigir a la página de login
      navigate('/login-red-global');
    }

    // Puedes hacer una llamada a tu API para obtener más información sobre el usuario usando el token JWT
    const fetchUserData = async () => {
      try {
        const response = await fetch('http://localhost:8000/api/user', {
          method: 'GET',
          headers: {
            'Authorization': `Bearer ${token}`,
          },
        });
        const data = await response.json();
        setUserData(data); // Guardar los datos del usuario en el estado
      } catch (error) {
        console.error('Error al obtener datos del usuario:', error);
      }
    };

    fetchUserData();
  }, [navigate]);

  return (
    <div className="min-h-screen bg-blue-100">
      <div className="container mx-auto p-8">
        <h1 className="text-3xl font-bold text-center text-blue-800 mb-6">Bienvenido a la Red Global de Recicladores</h1>
        {userData ? (
          <div className="bg-white p-6 rounded-md shadow-md">
            <h2 className="text-2xl font-semibold">Hola, {userData.username}!</h2>
            <p className="mt-4">Bienvenido a tu espacio en la Red Global de Recicladores. Aquí podrás gestionar tus tareas y paquetes.</p>

            {/* Agrega más contenido relacionado con la Red Global */}
            <div className="mt-6">
              <button
                onClick={() => navigate('/paquetes')}
                className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 transition"
              >
                Ver Paquetes
              </button>
            </div>
          </div>
        ) : (
          <p className="text-center text-xl text-gray-600">Cargando datos del usuario...</p>
        )}
      </div>
    </div>
  );
};

export default HomeRedGlobal;
