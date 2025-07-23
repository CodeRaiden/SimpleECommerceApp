import React, { useState, useEffect } from 'react'
import {
  login,
  getProducts,
  getCart,
  addOrUpdateCart,
  removeCart,
} from './services/api'
import LoginPage from './pages/LoginPage'
import ProductsPage from './pages/ProductPage'

function App() {
  const [authenticated, setAuth] = useState(false)
  const [products, setProducts] = useState([])
  const [cart, setCart] = useState([])

  useEffect(() => {
    if (authenticated) {
      getProducts().then(setProducts)
      getCart().then(setCart)
    }
  }, [authenticated])

  const handleLogin = async () => {
    await login()
    setAuth(true)
  }

  const handleAdd = async (pid) => {
    const exist = cart.find((i) => i.productId === pid)
    const qty = exist ? exist.quantity + 1 : 1
    await addOrUpdateCart({ productId: pid, quantity: qty })
    setCart(await getCart())
  }

  const handleRemove = async (pid) => {
    await removeCart(pid)
    setCart(await getCart())
  }

  if (!authenticated) return <LoginPage onLogin={handleLogin} />
  return (
    <ProductsPage
      products={products}
      cart={cart}
      onAdd={handleAdd}
      onRemove={handleRemove}
    />
  )
}

export default App
