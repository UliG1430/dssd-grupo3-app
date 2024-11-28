import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../service/apiService'; // Importamos el servicio de login
import LoginRedGlobalForm from '../components/LoginRedGlobalForm'; // Importamos el formulario

const LoginRedGlobal: React.FC = () => {
  const [error, setError] = useState<string | null>(null); // Estado para errores de login
  const [successMessage, setSuccessMessage] = useState<string | null>(null); // Estado para mensajes de éxito
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false); // Estado para controlar el estado de carga
  const navigate = useNavigate();

  // onSubmit que maneja el login
  const onSubmit = async (data: { username: string; password: string }) => {
    setIsSubmitting(true); // Activamos el estado de carga
    setError(null); // Limpiamos cualquier mensaje de error previo
    setSuccessMessage(null); // Limpiamos el mensaje de éxito anterior

    try {
      // Llamamos al servicio login y obtenemos la respuesta
      const response = await login(data.username, data.password);

      // Accedemos al token correctamente desde response.data.access_token
      const token = response.access_token;

      if (token) {
        localStorage.setItem('redGlobalToken', token); // Guardamos el token JWT
        localStorage.setItem('redGlobalUsername', data.username); // Guardamos el nombre de usuario
        // Redirigir a la página HomeRedGlobal
        navigate('/home-red-global'); // Redirigir sin importar el rol

        // Mostrar mensaje de éxito
        setSuccessMessage('Inicio de sesión exitoso');
      }
    } catch (error) {
      // Si hay un error, mostramos el mensaje de error
      setError('Hubo un problema al iniciar sesión. Verifica tus credenciales.');
    } finally {
      setIsSubmitting(false); // Desactivamos el estado de carga
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-blue-100">
      <div className="bg-white p-8 rounded-md shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center">Iniciar sesión - Red Global</h2>
        
        {/* Mostrar mensaje de éxito */}
        {successMessage && <div className="text-green-500 mb-4">{successMessage}</div>}
        
        {/* Mostrar mensaje de error */}
        {error && <div className="text-red-500 mb-4">{error}</div>}

        {/* Renderizar formulario de login */}
        <LoginRedGlobalForm
          onSubmit={onSubmit}
          isSubmitting={isSubmitting}
        />
      </div>
    </div>
  );
};

export default LoginRedGlobal;
