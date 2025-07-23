import axios from 'axios'
const api = axios.create({ baseURL: 'https://localhost:7226/api' })

export const login = async () => {
  // stub login, returns mock token
  api.defaults.headers.common['Authorization'] = 'Bearer demo-token'
}

export const getProducts = () =>
  api
    .get('/products')
    .then((r) => r.data)
    .catch((err) => {
      console.error('Error data:', err.response?.data)
      console.error('Error status:', err.response?.status)
    })
export const getCart = () =>
  api
    .get('/cart')
    .then((r) => r.data)
    .catch((err) => {
      console.error('Error data:', err.response?.data)
      console.error('Error status:', err.response?.status)
    })

export const addOrUpdateCart = (item) =>
  api.post('/cart', item).catch((err) => {
    console.error('Error data:', err.response?.data)
    console.error('Error status:', err.response?.status)
  })
export const removeCart = (productId) =>
  api.delete(`/cart/${productId}`).catch((err) => {
    console.error('Error data:', err.response?.data)
    console.error('Error status:', err.response?.status)
  })
