import React from 'react'
export default function Cart({ items, onRemove }) {
  const total = items.reduce((sum, i) => sum + i.quantity, 0)
  return (
    <div>
      <h2>Cart ({total} items)</h2>
      {items.map((i) => (
        <div key={i.productId}>
          <span>
            {i.productId} x {i.quantity}
          </span>
          <button onClick={() => onRemove(i.productId)}>Remove</button>
        </div>
      ))}
    </div>
  )
}
