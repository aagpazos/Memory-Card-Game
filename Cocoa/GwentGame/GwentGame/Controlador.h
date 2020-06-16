//
//  Controlador.h
//  GwentGame
//
//  Created by Adrian on 19/10/2019.
//  Copyright © 2019 Adrian A. G. Pazos. All rights reserved.
//

#import <Cocoa/Cocoa.h>
@class GameView;
@class PanelController;
@class Baraja;
NS_ASSUME_NONNULL_BEGIN

@interface Controlador : NSObject{
    Baraja *baraja;
    IBOutlet GameView *miVista;
    IBOutlet NSButton *boton;
    NSInteger cartasAbiertas;
    NSInteger numeroCartas;
    BOOL jugando;
    PanelController *panelController;
    IBOutlet NSButton *refreshButton;
}



-(IBAction)empezarJuego:(id)sender;

-(void)dibujarBaraja: (NSRect) b
 withGraphicsContext:(NSGraphicsContext *)ctx;
-(void)mouseEvent: (NSPoint) point
       withBounds: (NSRect) b
withGraphicsContext:(NSGraphicsContext *)ctx;

-(IBAction)showPanel:(id)sender;
-(IBAction)empezarDeNuevo:(id)sender;
-(void)reloadView;
-(void)finJuego;



@end

NS_ASSUME_NONNULL_END
