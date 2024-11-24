import React from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import InputField from '../components/InputField';
import { loginBonita, getUsuarioIdByUsername, getNextTaskId, getTaskById } from '../service/bonitaService';
import { getUsuarioByUsername } from '../service/UsuarioService';

interface LoginData {
  username: string;
  password: string;
}

const LoginForm: React.FC = () => {
  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<LoginData>();
  const navigate = useNavigate();

  const onSubmit = async (data: LoginData) => {
    try {
      const response = await loginBonita(data.username, data.password);
      if (response.token) {
        localStorage.setItem('bonitaToken', response.token);
        const idUserBonita = await getUsuarioIdByUsername(data.username);
        localStorage.setItem('idUserBonita', idUserBonita.toString());
        const idUser = await getUsuarioByUsername(data.username);
        localStorage.setItem('idUser', idUser.id.toString());
        
        if (!idUser.comenzoRecorrido)
            navigate('/comenzar-recoleccion');
        else {
            localStorage.setItem('caseId', idUser.caseId.toString());
            const nextTask = await getNextTaskId(idUser.caseId.toString());
            localStorage.setItem('nextTaskId', nextTask);
            const taskInfo = await getTaskById(nextTask);
            if (taskInfo.name === 'Visitar punto') {
                navigate('/visitar-punto');
            } else {
                if (taskInfo.name === 'Realizar orden') {
                    navigate('/cargar-recoleccion');
                }
            }
        }
      }
      //window.location.reload(); //para actualizar la navbar
    } catch (error) {
      alert('Hubo un problema al iniciar sesión');
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div className="bg-white p-8 rounded-md shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center">Iniciar Sesión</h2>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <InputField
            type="text"
            label="Username"
            placeholder="Ingrese su nombre de usuario"
            register={register}
            name="username"
            error={errors.username}
          />
          <InputField
            type="password"
            label="Password"
            placeholder="Ingrese su contraseña"
            register={register}
            name="password"
            error={errors.password}
          />
          <button
            type="submit"
            className="w-full bg-green-600 text-white p-2 rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
            disabled={isSubmitting}
          >
            {isSubmitting ? 'Iniciando sesión...' : 'Iniciar sesión'}
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginForm;