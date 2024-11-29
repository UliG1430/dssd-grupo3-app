// src/service/apiService.ts

interface Necesidad {
  deposit: string;
  material: string;
  quantity: number;
}

interface StockResponse {
  CodMaterial: string;
  StockActual: number;
}

// Usamos VITE_API_BASE_URL para necesidades y login
const API_URL = import.meta.env.VITE_API_BASE_URL;

// Usamos VITE_BACKEND_BASE_URL para las llamadas al backend
const BACKEND_URL = import.meta.env.VITE_BACKEND_BASE_URL;

// Función para realizar la llamada a la API de login
export const login = async (username: string, password: string): Promise<any> => {
  try {
    const response = await fetch(`${API_URL}/auth/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ username, password }),
    });

    if (!response.ok) {
      throw new Error('Error al autenticar usuario');
    }

    return await response.json();
  } catch (error) {
    console.error('Error en la llamada a login:', error);
    throw error;
  }
};

// Función para obtener las necesidades de los depósitos
export const getNecesidades = async (): Promise<Necesidad[]> => {
  try {
    const response = await fetch(`${API_URL}/api/necesidades`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error('Error al obtener las necesidades');
    }

    const data: Necesidad[] = await response.json();
    return data;
  } catch (error) {
    console.error('Error en la llamada a getNecesidades:', error);
    throw error;
  }
};

// Función para obtener el stock de un material por código
export const getStockMaterial = async (codMaterial: string): Promise<StockResponse> => {
  try {
    const response = await fetch(`${BACKEND_URL}/Material/${codMaterial}/stock`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error(`Error al obtener el stock del material ${codMaterial}`);
    }

    const data: StockResponse = await response.json();
    return data;
  } catch (error) {
    console.error(`Error en la llamada a getStockMaterial para ${codMaterial}:`, error);
    throw error;
  }
};