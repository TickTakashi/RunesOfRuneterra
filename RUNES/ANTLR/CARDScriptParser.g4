parser grammar CARDScriptParser;

options {
	tokenVocab=CARDScriptLexer;
}

cardDesc    : cardID DASH E? NUM cardALL cardE DODGE E? effect # cardDash 
            | cardID SKILL       cardALL cardE DODGE E? effect # cardSkill
            | cardID SPELL       cardALL cardE                  # cardSpell
            | cardID MELEE       cardALL cardE                  # cardMelee
            | cardID SELF E? NUM cardCL  cardE                  # cardSelf
            | cardID PASSIVE             cardE                  # cardPassive
            ;
             
cardID      : NAME E? NUM ;
cardE       : EFFECT E? effect ;
cardALL     : cardDR cardCL (preCond)?;
cardDR      : DAMAGE E? NUM RANGE E? NUM ;
cardCL      : COST   E? NUM LIMIT E? NUM ;

cardCC	    : CHANNEL | SLOW | SNARE | STUN | KNOCK | SILENCE | BLIND ;

effect		  : LBRACE stat RBRACE ;

preCond		  : ACTIVATE option WHEN condition;

option		  : OPTION | AUTO ;

stat		    : when                              # statWhen
            | action                            # statAction
			      | action value TIMES				        # actionRepeat
			      | stat stat								          # statList
			      ;

// TODO(ticktakashi): Think about the necessity for an IF-ELSE statement here.
when        : WHEN condition LBRACE stat RBRACE (value CHARGES)? ;

// TODO(ticktakashi): Think about how to increase the stats of a card here.
action      : cardCC NUM                                        # actionCC
            | player scalarEffect value                         # actionScalar  // e.g. Your opponent takes 4 damage. 
            | player ADDS value cardTarget FROM place TO place  # actionSearch  // e.g. Add one Mystic Shot from deck to hand
            ;

// TODO(ticktakashi): Add a (WITH card)? extension to these scalar to check if, for example, they did 2 damage with a MELEE attack
// TODO(ticktakashi): Add variables here. like binding card to a identifier x in cond Play
// TODO(ticktakashi): Think about adding conditions for card drawing and cooldown etc. (Seems obscure, add as necessary).
condition	  : player PLAYS cardTarget (IDENT)?  # condPlay    // TODO(ticktakashi): Implement IDENT component of this.
            | player scalarEffect ineq value		# condScalar
			      | condition binopBool condition			# condExpr
			      | NOT condition							        # condNot
			      | LPAREN condition RPAREN				    # condParen
			      ;

player		  : USER | ENEMY ;

scalarEffect: DRAWS | TAKES | HEALS | SHIELDS | PIERCES;


// TODO(ticktakashi): Implement semantics for places. Like the hand, cooldown, and the deck.
place       : player location ;

location    : HAND | DECK | COOL ;

// TODO(ticktakashi): Think about the IDENT case. Need to keep a symbol table.
cardTarget	: ALL | MELEE | SKILL | SPELL | DASH | PASSIVE | NUM | THIS | IDENT ; 

// TODO(ticktakashi): Add Card Based Values, Like x card's damage value.
value		    : NUM                  # valueInt
            | value TILDE value       # valueRandom
            | IDENT DOT cardProperty  # valueCard   // TODO(ticktakashi): x.SHIELD would mean "equal to x's shield"
            | cardTarget IN place     # valueCount  // TODO(ticktakashi): ALL IN USER HAND would mean "equal to the number of cards in your hand"
            | HALF value              # valueHalf   // TODO(ticktakashi): HALF ALL IN USER HAND would mean "equal to half the number of cards in your hand"
            | DOUBLE value            # valueDouble // TODO(ticktakashi): DOUBLE x.SHIELD would mean "equal to double x's shield"
            | DISTANCE                # valueDistance // TODO(ticktakashi): The distance between players
            ;

cardProperty: DAMAGE | RANGE | COST;

binopBool	  : OR | AND  ;

ineq		    : GT | LT | EQ ;