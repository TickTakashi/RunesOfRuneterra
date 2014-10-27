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


effect		  : LBRACE (CHANNEL NUM)? stat RBRACE ;

preCond		  : ACTIVATE option WHEN eventCond;

option		  : OPTION | AUTO ;

stat		    : if                                # statIf
            | when                              # statWhen
            | action                            # statAction
			      | action value TIMES				        # actionRepeat
			      | stat stat								          # statList
			      ;

// Triggers an effect the next time that whenCond occurs.
when        : WHEN eventCond LBRACE stat RBRACE (value CHARGES)? ;

// TODO(ticktakashi): Checks if whenCond is true when this card is activated.
if          : IF stateCond LBRACE stat RBRACE (ELSE LBRACE stat RBRACE)? ;

// TODO(ticktakashi): Implement if conditions
stateCond   : player HEALTH ineq value  #stateCondHealth // If the enemy has less than x health
            ; // TODO(ticktakashi): Melee attacked 3 times in the past 3 turns - Will require some kind of action history unless it is constantly counted.
              // This will be a stateCond which checks for historical eventCond.
              // Store all fired events in an ordered list and then search back till you find what you're looking for or reach some negtive conditions
              // Options like last n turns or n turns in a row or n times in a row or last action etc.
    
// TODO(ticktakashi): Think about how to increase the stats of a card here.
// TODO(ticktakashi): Philosophy: Implement the general case, and then
//                    implement special cases as special cases,
//                    since those will still be combinatorially useful and
//                    it will save a bunch of time and complexity in 
//                    frequently run code.
action      : ccEffect NUM                                      # actionCC
            | player scalarEffect value                         # actionScalar  // e.g. Your opponent takes 4 damage. 
            | player ADDS value cardTarget FROM place TO place  # actionSearch  // e.g. Add one Mystic Shot from deck to hand
            ;

// TODO(ticktakashi): Add a (WITH card)? extension to these scalar to check if, for example, they did 2 damage with a MELEE attack
// TODO(ticktakashi): Add variables here. like binding card to a identifier x in cond Play
// TODO(ticktakashi): Think about adding whenConds for card drawing and cooldown etc. (Seems obscure, add as necessary).
eventCond	  : player PLAYS cardTarget (IDENT)?  # eventCondPlay    // TODO(ticktakashi): Implement IDENT component of this.
            | player scalarEffect ineq value		# eventCondScalar
			      | eventCond binopBool eventCond     # eventCondExpr
			      | NOT eventCond							        # eventCondNot
			      | LPAREN eventCond RPAREN				    # eventCondParen
			      ;

player		  : USER | ENEMY ;

// TODO(ticktakashi): Remember that in LoL, there are no skills which positively effect your opponent.
//                    However, there are skills which negatively effect the user.
//                    Also, when checking whenConds we must consider that the opponent might heal
//                    himself etc.
scalarEffect: DRAWS | TAKES | HEALS | SHIELDS | PIERCES;

ccEffect	  : SLOW | SNARE | STUN | KNOCK | SILENCE | BLIND ;


// TODO(ticktakashi): Implement semantics for places. Like the hand, cooldown, and the deck.
place       : player location ;

location    : HAND | DECK | COOL ;

// TODO(ticktakashi): Think about the IDENT case. Need to keep a symbol table.
cardTarget	: ALL | MELEE | SKILL | SPELL | DASH | PASSIVE | NUM | THIS | IDENT ; 

// TODO(ticktakashi): Add Card Based Values, Like x card's damage value.
value		    : NUM                     # valueInt
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