// src/components/Navbar.tsx
import React from 'react';
import { Link } from 'react-router-dom';

const Navbar: React.FC = () => {
  return (
    <nav className="bg-green-600 p-4 shadow-md">
      <div className="container mx-auto flex justify-between items-center">
        {/* Hacemos que el título "Ecocycle" sea un enlace a la página de inicio */}
        <Link to="/" className="text-white text-2xl font-bold">
          Ecocycle
        </Link>
        <div className="space-x-4">
          <Link
            to="/cargar-recoleccion"
            className="text-white hover:bg-green-700 px-3 py-2 rounded-md text-lg"
          >
            Cargar Recolección
          </Link>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
