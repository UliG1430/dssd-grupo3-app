interface Necesidad {
  id: number;
  material: string;
  cod_material: string;
  quantity: number;
  deposito_id: number;
  material_id: number;
  estado: string;
}

interface OrdenDistribucion {
  id: number;
  necesidad_id: number;
  estado: string;
}

// URLs base desde las variables de entorno
const API_URL = import.meta.env.VITE_API_BASE_URL; // URL para la API Cloud
const BACKEND_URL = import.meta.env.VITE_BACKEND_BASE_URL; // URL para el backend local

// Función para obtener el token almacenado en localStorage
const getAuthToken = (): string | null => {
  return localStorage.getItem('redGlobalToken');
};

/// Servicio de login
export const login = async (
  username: string,
  password: string
): Promise<{ access_token: string; rol: string }> => {
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

    const data = await response.json();

    // Validar que la respuesta contiene los campos necesarios
    if (!data.access_token || !data.rol) {
      throw new Error('La respuesta del servidor no contiene el token o el rol.');
    }

    return data; // Devolvemos el token y el rol
  } catch (error) {
    console.error('Error en el servicio de login:', error);
    throw error;
  }
};

// Función para obtener las necesidades de los depósitos
export const getNecesidades = async (): Promise<Necesidad[]> => {
  const token = getAuthToken();
  try {
    const response = await fetch(`${API_URL}/api/necesidades`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error('Error al obtener las necesidades');
    }

    const data = await response.json();
    console.log('Respuesta de getNecesidades:', data);

    return data.map((item: any) => ({
      id: item.id,
      material: item.material,
      cod_material: item.CodMaterial,
      quantity: item.quantity,
      deposito_id: item.deposito_id,
      material_id: item.material_id,
      estado: item.estado,
    }));
  } catch (error) {
    console.error('Error en la llamada a getNecesidades:', error);
    throw error;
  }
};

// Función para obtener el stock de un material
export const getStockMaterial = async (codMaterial: string): Promise<number> => {
  const token = getAuthToken();
  try {
    const response = await fetch(`${BACKEND_URL}/Material/${codMaterial}/stock`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error(`Error al obtener el stock del material ${codMaterial}`);
    }

    const data = await response.json();
    console.log('Stock del material:', data);

    return data.stockActual || 0;
  } catch (error) {
    console.error(`Error en la llamada a getStockMaterial (${codMaterial}):`, error);
    throw error;
  }
};

// Verificar si un depósito está registrado como proveedor de un material
export const checkDepositoProveedor = async (materialId: number, depositoId: number): Promise<boolean> => {
  const token = getAuthToken();
  try {
    const response = await fetch(`${API_URL}/api/check_combination?material_id=${materialId}&deposito_id=${depositoId}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error('Error al verificar combinación de depósito y material');
    }

    const data = await response.json();
    console.log('Resultado de checkDepositoProveedor:', data);

    return data.exists;
  } catch (error) {
    console.error('Error en la llamada a checkDepositoProveedor:', error);
    throw error;
  }
};

// Registrar un proveedor en la API
export const addDepositoProveedor = async (depositoId: number, materialId: number, codigoMaterial: string): Promise<void> => {
  const token = getAuthToken();
  try {
    const response = await fetch(`${API_URL}/api/add_deposito_proveedor`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        deposito_id: depositoId,
        material_id: materialId,
        codigo_material: codigoMaterial,
      }),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.msg || 'Error al registrar el proveedor');
    }
    console.log('Proveedor registrado exitosamente.');
  } catch (error) {
    console.error('Error en la llamada a addDepositoProveedor:', error);
    throw error;
  }
};

// Tomar una necesidad y crear la orden de distribución
export const tomarNecesidad = async (necesidadId: number): Promise<void> => {
  const token = getAuthToken();
  try {
    const response = await fetch(`${API_URL}/api/necesidades/tomar/${necesidadId}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.msg || 'Error al tomar la necesidad');
    }

    console.log(`Necesidad ${necesidadId} tomada exitosamente.`);
  } catch (error) {
    console.error('Error al tomar la necesidad:', error);
    throw error;
  }
};

// Reducir el stock de un material
export const reduceMaterialStock = async (codMaterial: string, cantidad: number): Promise<void> => {
console.log('Entrada de reduceMaterialStock:', codMaterial, cantidad);
  try {
    // Llamada al backend con codMaterial
    const response = await fetch(`${BACKEND_URL}/Material/Stock/Reduce/${codMaterial}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ Cantidad: cantidad }), // Payload con la cantidad a reducir
    });

    if (!response.ok) {
      const errorData = await response.text();
      console.error('Error al reducir el stock:', errorData);
      throw new Error(errorData);
    }

    console.log(`Stock del material ${codMaterial} reducido en ${cantidad}.`);
  } catch (error) {
    console.error(`Error al reducir el stock del material ${codMaterial}:`, error);
    throw error;
  }
};

// Función para obtener las órdenes de distribución desde la API
export const getOrdenesDistribucion = async (): Promise<OrdenDistribucion[]> => {
  const token = localStorage.getItem('redGlobalToken'); // Obtener el token del almacenamiento local
  if (!token) {
    throw new Error('Token no encontrado. Por favor inicia sesión.');
  }

  try {
    const response = await fetch(`${API_URL}/api/ordenes-distribucion`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`, // Incluye el token en el encabezado
      },
    });

    if (!response.ok) {
      const error = await response.text(); // Capturar el error en texto
      throw new Error(`Error al obtener las órdenes de distribución: ${error}`);
    }

    // Parsear y devolver las órdenes
    const data: OrdenDistribucion[] = await response.json();
    return data;
  } catch (error) {
    console.error('Error en la llamada a getOrdenesDistribucion:', error);
    throw error;
  }
};

export const tomarOrdenDistribucion = async (ordenId: number): Promise<void> => {
  const token = localStorage.getItem('redGlobalToken');
  console.log('Tomando orden de distribución:', ordenId);
  const response = await fetch(`${API_URL}/api/ordenes_distribucion/tomar/${ordenId}`, {
    method: 'PATCH',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.msg || 'Error al tomar la orden de distribución');
  }
};

export const confirmarEntregaOrdenDistribucion = async (ordenId: number): Promise<void> => {
  const token = localStorage.getItem('redGlobalToken');
  try {
    const response = await fetch(`${API_URL}/api/ordenes_distribucion/confirmar/${ordenId}`, {
      method: 'PATCH',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.msg || 'Error al confirmar la entrega');
    }

    console.log(`Orden ${ordenId} confirmada como distribuida.`);
  } catch (error) {
    console.error(`Error al confirmar la entrega de la orden ${ordenId}:`, error);
    throw error;
  }
};