import React from 'react'
import ProductItem from '../components/ProductItem'
import Cart from '../components/Cart'

export default function ProductsPage({ products, cart, onAdd, onRemove }) {
  return (
    <div style={{ display: 'flex' }}>
      <div style={{ flex: 2 }}>
        <h2>Products</h2>
        {products.map((p) => (
          <ProductItem key={p.id} product={p} onAdd={() => onAdd(p.id)} />
        ))}
      </div>
      <div style={{ flex: 1 }}>
        <Cart items={cart} onRemove={onRemove} />
      </div>
    </div>
  )
}
