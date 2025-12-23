import axios from 'axios';

const API_BASE_URL = 'http://localhost:5147/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor to handle errors
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('refreshToken');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const authAPI = {
  login: async (userName, password) => {
    const response = await api.post('/Token', { userName, password });
    return response.data;
  },
  refreshToken: async (token, refreshToken) => {
    const response = await api.post('/RefreshToken', { token, refreshToken });
    return response.data;
  },
};

export const usersAPI = {
  getAll: async () => {
    const response = await api.get('/Users');
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/Users/${id}`);
    return response.data;
  },
  create: async (userData) => {
    const response = await api.post('/Users', userData);
    return response.data;
  },
  update: async (userData) => {
    const response = await api.put('/Users', userData);
    return response.data;
  },
  delete: async (id) => {
    const response = await api.delete(`/Users/${id}`);
    return response.data;
  },
  getUserLocations: async () => {
    const response = await api.get('/Users/GetUserLocations');
    return response.data;
  },
};

export default api;

