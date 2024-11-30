import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../service/apiService'; // Servicio de login
import LoginRedGlobalForm from '../components/LoginRedGlobalForm'; // Componente del formulario

const LoginRedGlobal: React.FC = () => {
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const navigate = useNavigate();

  const onSubmit = async (data: { username: string; password: string }) => {
    setIsSubmitting(true);
    setError(null);
    setSuccessMessage(null);

    try {
      // Llamamos al servicio de login
      const response = await login(data.username, data.password);

      // Accedemos al token y al rol del usuario
      const { access_token: token, rol } = response;

      if (token) {
        // Guardamos el token y el rol en localStorage
        localStorage.setItem('redGlobalToken', token);
        localStorage.setItem('userRol', rol);

        // Redirigimos según el rol del usuario
        if (rol === 'A') {
          navigate('/necesidades'); // Página para administradores o rol A
        } else if (rol === 'D') {
          navigate('/ordenes-distribucion'); // Página para distribuidores o rol D
        } else {
          setError('Rol de usuario no reconocido.');
        }

        // Mostrar mensaje de éxito
        setSuccessMessage('Inicio de sesión exitoso');
      }
    } catch (error) {
      console.error('Error en el login:', error);
      setError('Hubo un problema al iniciar sesión. Verifica tus credenciales.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-blue-100">
      <div className="bg-white p-8 rounded-md shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center">Iniciar sesión - Red Global</h2>

        {/* Mensaje de éxito */}
        {successMessage && <div className="text-green-500 mb-4">{successMessage}</div>}

        {/* Mensaje de error */}
        {error && <div className="text-red-500 mb-4">{error}</div>}

        {/* Formulario de login */}
        <LoginRedGlobalForm onSubmit={onSubmit} isSubmitting={isSubmitting} />
      </div>
    </div>
  );
};

export default LoginRedGlobal;