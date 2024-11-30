interface Necesidad {
  id: number;
  material: string;
  cod_material: string;
  quantity: number;
  deposito_id: number;
  material_id: number;
  estado: string;
}

// URLs base desde las variables de entorno
const API_URL = import.meta.env.VITE_API_BASE_URL; // URL para la API Cloud
const BACKEND_URL = import.meta.env.VITE_BACKEND_BASE_URL; // URL para el backend local

// Función para obtener el token almacenado en localStorage
const getAuthToken = (): string | null => {
  return localStorage.getItem('redGlobalToken');
};

// Función para realizar la llamada a la API de login
export const login = async (username: string, password: string): Promise<any> => {
  try {
    console.log('Entrada de login:', username, password);

    const response = await fetch(`${API_URL}/auth/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ username, password }),
    });
    console.log('Respuesta de login:', response);

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
  const token = getAuthToken(); // Obtén el token del localStorage
  try {
    const response = await fetch(`${API_URL}/api/necesidades`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`, // Incluye el token en el encabezado
      },
    });

    if (!response.ok) {
      throw new Error('Error al obtener las necesidades');
    }

    const data: any[] = await response.json();
    console.log('Respuesta de getNecesidades cruda:', data); // Log para depurar

    // Normalizamos las claves del objeto recibido
    const normalizedData: Necesidad[] = data.map((item) => ({
      id: item.id,
      material: item.material,
      cod_material: item.CodMaterial, // Renombramos correctamente el campo
      quantity: item.quantity,
      deposito_id: item.deposito_id,
      material_id: item.material_id,
      estado: item.estado,
    }));

    console.log('Datos normalizados de getNecesidades:', normalizedData); // Log para depurar
    return normalizedData;
  } catch (error) {
    console.error('Error en la llamada a getNecesidades:', error);
    throw error;
  }
};

// Función para obtener el stock de un material por código desde el backend local
export const getStockMaterial = async (codMaterial: string): Promise<number> => {
  const token = getAuthToken(); // Obtén el token del localStorage
  console.log(`Consultando stock para material: ${codMaterial}`);

  try {
    const response = await fetch(`${BACKEND_URL}/Material/${codMaterial}/stock`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`, // Incluye el token en el encabezado
      },
    });

    console.log('Respuesta completa:', response);

    if (!response.ok) {
      const errorText = await response.text(); // Leer el texto para inspeccionar el error
      console.error('Error al obtener stock:', errorText);
      throw new Error(`Error al obtener el stock del material ${codMaterial}`);
    }

    const data = await response.json();
    console.log('StockResponse recibido del backend:', data);

    // Retorna solo el valor del stockActual
    return data.stockActual || 0; // Si stockActual es undefined, retorna 0 como valor por defecto
  } catch (error) {
    console.error(`Error en la llamada a getStockMaterial para ${codMaterial}:`, error);
    throw error;
  }
};

// Función para verificar si un depósito está registrado como proveedor de un material
export const checkDepositoProveedor = async (materialId: number, depositoId: number): Promise<boolean> => {
  const token = getAuthToken(); // Obtén el token del localStorage
  try {
    const response = await fetch(`${API_URL}/api/check_combination?material_id=${materialId}&deposito_id=${depositoId}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`, // Incluye el token en el encabezado
      },
    });

    if (!response.ok) {
      const data = await response.json();
      throw new Error(data.msg || 'Error al verificar combinación de depósito y material');
    }

    const data = await response.json();
    console.log('Respuesta de checkDepositoProveedor:', data);

    return data.exists; // Devuelve el booleano proporcionado por la API
  } catch (error) {
    console.error('Error en la llamada a checkDepositoProveedor:', error);
    throw error;
  }
};

// Función para registrar un proveedor en la API
export const addDepositoProveedor = async (
  depositoId: number,
  materialId: number,
  codigoMaterial: string
): Promise<void> => {
  const token = getAuthToken(); // Obtén el token del localStorage
  try {
    const response = await fetch(`${API_URL}/api/add_deposito_proveedor`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`, // Incluye el token en el encabezado
      },
      body: JSON.stringify({
        deposito_id: depositoId,
        material_id: materialId,
        codigo_material: codigoMaterial,
      }),
    });

    if (!response.ok) {
      const errorText = await response.text(); // Leer el texto para inspeccionar el error
      console.error('Error al registrar proveedor:', errorText);
      const data = await response.json();
      throw new Error(data.msg || 'Error al registrar el proveedor');
    }
  } catch (error) {
    console.error('Error en la llamada a addDepositoProveedor:', error);
    throw error;
  }
};

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
      const data = await response.json();
      throw new Error(data.msg || 'Error al tomar la necesidad');
    }

    console.log(`Necesidad ${necesidadId} tomada con éxito.`);
  } catch (error) {
    console.error('Error al tomar la necesidad:', error);
    throw error;
  }
};