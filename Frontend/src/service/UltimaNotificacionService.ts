

export interface UltimaRecoleccion {
  id: number;
  fecha: string;
}

export const getUltimaNotificacion = async (): Promise<UltimaRecoleccion> => {
  try {
    const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/UltimaEvaluacion`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error('Error fetching ultima recoleccion');
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error fetching ultima recoleccion:', error);
    throw error;
  }
};

export const updateUltimaNotificacion = async (): Promise<void> => {
  try {
    const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/UltimaEvaluacion/SetFechaToNow`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error('Error updating ultima recoleccion');
    }

    console.log('Ultima recoleccion updated to today');
  } catch (error) {
    console.error('Error updating ultima recoleccion:', error);
    throw error;
  }
};