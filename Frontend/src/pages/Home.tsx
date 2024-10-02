import React from 'react';

const Home: React.FC = () => {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-green-50">
      <h1 className="text-4xl font-bold text-green-800 mb-4">Bienvenido a Ecocycle</h1>
      <p className="text-xl text-gray-700">
        Nuestro compromiso es con el reciclaje y un futuro sostenible.
      </p>
    </div>
  );
};

export default Home;
