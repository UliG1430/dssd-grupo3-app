export interface Evaluacion {
    Id: number;
    caseId: number;
    state: string;
    observaciones: string;
    cantOrdenes: number;
    cantOrdenesOk: number;
    cantOrdenesMal: number;
  }
  
  export const getEvaluacion = async (caseId: any): Promise<Evaluacion> => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Evaluacion/GetEvaluacion/${caseId}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });
  
      if (response.status != 200) {
        throw new Error('Error fetching evaluacion');
      }
  
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Error fetching evaluacion:', error);
      throw error;
    }
  };
  
  export const updateEvaluacion = async (datos: any): Promise<void> => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Evaluacion/UpdateEvaluacion`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(datos),
      });
  
      if (response.status != 200) {
        throw new Error('Error updating evaluacion state');
      }
  
      console.log('Evaluacion updated');
    } catch (error) {
      console.error('Error updating evaluacion state:', error);
      throw error;
    }
  };
  
  export const addEvaluacion = async (caseId: string): Promise<void> => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Evaluacion/AddEvaluacion/${caseId}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(caseId),
      });
  
      if (response.status != 201) {
        throw new Error('Error adding evaluacion');
      }
  
      console.log('Evaluacion added');
    } catch (error) {
      console.error('Error adding evaluacion:', error);
      throw error;
    }
  };

  export const getEvaluaciones = async (): Promise<Evaluacion[]> => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Evaluacion/GetAllWithEnvState`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });
  
      if (response.status != 200) {
        throw new Error('Error fetching evaluaciones');
      }
  
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Error fetching evaluaciones:', error);
      throw error;
    }
  }