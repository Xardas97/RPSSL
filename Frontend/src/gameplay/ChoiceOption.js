import "./styles.css";

export default function ChoiceOption({ value, onShapeClick }) {
  return (
    <img className="shape-choice" src={require('./images/' + value + '.png')} onClick={onShapeClick}/>
  );
}