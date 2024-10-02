// src/pages/CreatePlanPage.tsx
import React from 'react';
import PlanForm from '../components/PlanForm'; // Importa el componente del formulario

const CreatePlanPage: React.FC = () => {
  return (
    <div className="min-h-screen bg-green-50 flex items-center justify-center">
      <div className="bg-white p-8 rounded-lg shadow-lg max-w-md w-full">
        <h1 className="text-3xl font-bold text-center mb-6 text-green-800">Crear Plan de Recolección</h1>
        <PlanForm />
      </div>
    </div>
  );
};

export default CreatePlanPage;
