import axios from 'axios';

const API_URL = 'http://localhost:8000'; // URL base de tu API

// Función para realizar la llamada a la API de login
export const login = async (username: string, password: string) => {
  try {
    const response = await axios.post(`${API_URL}/auth/login`, {
      username,
      password,
    });
    console.log("response", response);
    return response.data; 
  } catch (error) {
    console.error('Error al realizar el login:', error);
    throw new Error('Error al autenticar usuario');
  }
};
