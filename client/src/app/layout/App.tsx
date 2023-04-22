import React, { useEffect, useState } from 'react';
import { Product } from '../models/product';
import Catalog from '../../features/catalog/Catolog';


function App() {

  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    fetch('http://localhost:5000/api/products')
    .then(response => response.json())
    .then( data => setProducts(data))
  }, [])

  function addProduct() {
    setProducts(prevState => [...prevState, ({id: (prevState.length) + 1, name: 'product ' + (prevState.length + 1), 
      description: 'description ' + (prevState.length) + 1, brand: 'brand ' + (prevState.length) + 1, pictureUrl: 'src/logo.svg', 
      quantityInStock: 100, type: 'type ' + (prevState.length) + 1, price: (prevState.length * 100) + 100})]);
  }

  return (
    <div>
      <h1>Re-Store</h1>
      <Catalog products={products} addProduct={addProduct} />
    </div>
  );
}

export default App;
