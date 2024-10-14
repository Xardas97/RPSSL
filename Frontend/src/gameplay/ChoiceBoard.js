import ChoiceOption from './ChoiceOption';
import "./styles.css";

export default function ChoiceBoard({shapes, playShape}) {
  async function handleClick(shape) {
    await playShape(shape)
  }

  const choiceButtons = shapes.map(choice =>
    <ChoiceOption key={choice.id} value={choice.name} onShapeClick={async () => await handleClick(choice)} />
  )

  return (
    <div className="choice-board">
      {choiceButtons}
    </div>
  );
}