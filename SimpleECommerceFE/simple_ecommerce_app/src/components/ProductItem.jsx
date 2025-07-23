import React from 'react'
export default function ProductItem({ product, onAdd }) {
  return (
    <div style={{ border: '1px solid #ccc', padding: 8, margin: 4 }}>
      <h3>{product.name}</h3>
      <p>{product.description}</p>
      <p>${product.price.toFixed(2)}</p>
      <button onClick={onAdd}>Add to cart</button>
    </div>
  )
}
