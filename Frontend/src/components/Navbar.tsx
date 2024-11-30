import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { logoutBonita } from '../service/bonitaService';

const Navbar: React.FC = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isAdmin, setIsAdmin] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('bonitaToken');
    const userRole = localStorage.getItem('userRol');

    if (token) {
      setIsLoggedIn(true);
    }
    if (userRole === 'A') {
      setIsAdmin(true);
    }
  }, []);

  const handleLogout = async () => {
    try {
      // Llamar al servicio de logout para realizar una limpieza si es necesario
      await logoutBonita();

      // Limpiar localStorage y el estado
      localStorage.removeItem('bonitaToken');
      localStorage.removeItem('userRol');

      // Actualizar estado local
      setIsLoggedIn(false);
      setIsAdmin(false);

      // Redirigir al home o página principal
      navigate('/');
    } catch (error) {
      console.error('Error al cerrar sesión:', error);
    }
  };

  return (
    <nav className="bg-green-600 p-4 shadow-md">
      <div className="container mx-auto flex justify-between items-center">
        <Link to="/" className="text-white text-2xl font-bold">
          Ecocycle
        </Link>
        <div className="space-x-4">
          {isAdmin && (
            <>
              <Link
                to="/evaluaciones"
                className="text-white hover:bg-green-700 px-3 py-2 rounded-md text-lg"
              >
                Evaluaciones
              </Link>
              <Link
                to="/paquetes"
                className="text-white hover:bg-green-700 px-3 py-2 rounded-md text-lg"
              >
                Paquetes
              </Link>
              <Link
                to="/red-global-recicladores"
                className="text-white hover:bg-green-700 px-3 py-2 rounded-md text-lg"
              >
                Red Global de Recicladores
              </Link>
            </>
          )}
          {!isLoggedIn ? (
            <Link
              to="/iniciar-sesion"
              className="text-white hover:bg-green-700 px-3 py-2 rounded-md text-lg"
            >
              Iniciar Sesión
            </Link>
          ) : (
            <button
              onClick={handleLogout}
              className="text-white hover:bg-green-700 px-3 py-2 rounded-md text-lg"
            >
              Cerrar Sesión
            </button>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
