import { Product } from "../../app/models/product"

interface Props{
    products: Product[];
    addProduct: () => void;
}

export default function Catalog({products, addProduct}: Props){
    return (
        <>
            <ol>
                {products.map((product, index) => (
                    <li key={index}>
                        {product.name} - {product.price.toFixed(2)}
                    </li>
                ))}
            </ol>
            <button onClick={addProduct}>Add Product</button>
        </>
    )
}